using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{

    public float defaultSize; 
    public float zoomedSize;

    public string namePlayer;

    public bool isSelling;

    public ShopItemData buyingItem;

    public InventoryManager inventoryManager;

    private TileManager tileManager;

    private List<PlantHolder> plantHolders = new List<PlantHolder>();

    private GardenManager gardenManager;

    public Camera mainCamera;

    public PlantableStepsConfigurations plantStepsConfigurations;

    private List<PlantHolder> plantsToHarvest = new List<PlantHolder>();

    private List<PlantHolder> growingPlants = new List<PlantHolder>();

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
        gardenManager = GameManager.instance.gardenManager;
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (buyingItem != null)
            {
                int balance = GameManager.instance.moneyManager.GetBalance();
                if(balance  >= buyingItem.price)
                {
                    inventoryManager.Add("Backpack", buyingItem.item);
                    GameManager.instance.moneyManager.RemoveMoney(buyingItem.price);
                }
            }
            if(isSelling && inventoryManager.toolbar.selectedSlot != null && inventoryManager.toolbar.selectedSlot.itemData.isSellable)
            {
                GameManager.instance.moneyManager.AddMoney(inventoryManager.toolbar.selectedSlot.itemData.price);
                inventoryManager.toolbar.selectedSlot.RemoveItem();
                GameManager.instance.uiManager.RefreshAll();
            }
            if(inventoryManager.toolbar.selectedSlot.itemData != null)
            {
                Vector3Int pos = new Vector3Int((int)(transform.position.x),
                (int)(transform.position.y),
                0);

                string tileName = tileManager.GetTileName(pos);

                if (!string.IsNullOrWhiteSpace(tileName))
                {
                    if (tileName == "tiles_48" && inventoryManager.toolbar.selectedSlot.itemData.itemType == ItemType.Hoe)
                    {
                        tileManager.SetInteracted(pos);
                    }
                    else if (tileName == "tiles_451" && inventoryManager.toolbar.selectedSlot.itemData.itemType == ItemType.Showel)
                    {
                        // Создаем грядку с использованием менеджера грядок
                        gardenManager.CreateGardenBed(pos);
                    }
                }
            }
        }



        if (Input.GetKeyUp(KeyCode.F))
        {
            Inventory.Slot slot = GameManager.instance.inventoryManager.toolbar.selectedSlot;
            ItemData itemData = slot.itemData;
            if(itemData != null)
            {
                if (itemData.isPlantable)
                {
                    if (plantHolders.Count != 0)
                    {
                        PlantableSteps plantableSteps = plantStepsConfigurations.stepsConfigurations.FirstOrDefault(stepsConfig => stepsConfig.itemType == itemData.itemType);
                        if (plantableSteps != null)
                        {
                            plantHolders[^1].plantableSteps = plantableSteps;

                            plantHolders[^1].StartGrow();

                            slot.RemoveItem();

                            GameManager.instance.uiManager.RefreshAll();

                            plantHolders.Remove(plantHolders[^1]);
                        }
                    }
                }
                if (itemData.itemType == ItemType.Fertilizer)
                {
                    if (growingPlants.Count != 0)
                    {
                        growingPlants[^1].isFertilized = true;
                        slot.RemoveItem();
                        GameManager.instance.uiManager.RefreshAll();
                        growingPlants.Remove(growingPlants[^1]);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            if (plantsToHarvest.Count != 0)
            {
                int count = plantsToHarvest[^1].isFertilized ? plantsToHarvest[^1].fertilizedCount : plantsToHarvest[^1].cropCount;
                for (int i = 0; i < count; i++)
                {
                    Instantiate(plantsToHarvest[^1].cropPrefab, plantsToHarvest[^1].transform.position, Quaternion.identity);
                }

                Vector3Int position = new Vector3Int((int)plantsToHarvest[^1].transform.position.x, (int)plantsToHarvest[^1].transform.position.y, 0);

                plantsToHarvest.Remove(plantsToHarvest[^1]);
                
                gardenManager.RemoveGardenBed(position);    

                GameManager.instance.uiManager.RefreshAll();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            mainCamera.orthographicSize = zoomedSize;
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            mainCamera.orthographicSize = defaultSize;
        }

    }
    public void DropItem(Item item)
    {
        Vector2 spawnPosition = transform.position;

        float radius = 1.5f;

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        Vector2 randomOnCircle = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * radius;

        Item droppedItem = Instantiate(item, spawnPosition + randomOnCircle, Quaternion.identity);

        droppedItem.rb2d.AddForce(randomOnCircle.normalized * 0.5f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlantHolder plantHolder))
        {
            if(plantHolder.plantableSteps == null)
            {
                plantHolders.Add(plantHolder);
            }
            else if(plantHolder.plantableSteps != null && !plantHolder.readyToHarvest)
            {
                growingPlants.Add(plantHolder);
            }
            else if(plantHolder.readyToHarvest)
            {
                plantsToHarvest.Add(plantHolder);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlantHolder plantHolder))
        {
            if (plantHolders.Contains(plantHolder))
            {
                plantHolders.Remove(plantHolder);
            }
            if (growingPlants.Contains(plantHolder))
            {
                growingPlants.Remove(plantHolder);
            }
            if (plantsToHarvest.Contains(plantHolder))
            {
                plantsToHarvest.Remove(plantHolder);
            }
        }

    }

    public void SetName(string name)
    {
        this.name = name;
        GameManager.instance.uiManager.UpdatePlayerName();
    }
}
