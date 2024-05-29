using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;

    public TileManager tileManager;

    public TilemapManager tilemapManager;

    public UI_Manager uiManager;

    public SavesManager saveManager;

    public InventoryManager inventoryManager;

    public GardenManager gardenManager;

    public WorldGenerator worldGenerator;

    public Player player;

    public MenuController menuController;

    public MoneyManager moneyManager;

    public GameObject appleTreePrefab;
    public GameObject oakTreePrefab;
    public GameObject pineTreePrefab;

    public GameObject gardenBedPrefab;

    public GameObject bush1Prefab;
    public GameObject bush2Prefab;

    public GameObject flower1Prefab;
    public GameObject flower2Prefab;
    public GameObject flower3Prefab;
    public GameObject flower4Prefab;
    public GameObject flower5Prefab;
    public GameObject flower6Prefab;
    public GameObject flower7Prefab;

    public GameObject shopPrefab;
    public GameObject homePrefab;

    public PlantableStepsConfigurations plantStepsConfigurations;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        player = FindObjectOfType<Player>();

        inventoryManager = GetComponent<InventoryManager>();

        uiManager = GetComponent<UI_Manager>();

        itemManager = GetComponent<ItemManager>();

        tileManager = GetComponent<TileManager>();

        tilemapManager = GetComponent<TilemapManager>();

        saveManager = GetComponent<SavesManager>();

        gardenManager = GetComponent<GardenManager>();

        menuController = GetComponent<MenuController>();

        worldGenerator = GetComponent<WorldGenerator>();

        moneyManager = GetComponent<MoneyManager>();
    }

    private void Start()
    {
        GameData gameData = SaveSystem.LoadGame();
        if (gameData != null)
        {
            if (!gameData.IsNewGame)
            {
                uiManager.ToggleInventoryUI();
                LoadGame();
                uiManager.RefreshAll();
            }
        }
    }

    public void SaveGame()
    {
        if (saveManager != null && player != null && tileManager != null)
        {
            Inventory backpack = GameManager.instance.inventoryManager.GetInventoryByName("Backpack");
            Inventory toolbar = GameManager.instance.inventoryManager.GetInventoryByName("Toolbar");

            int money = moneyManager.GetBalance();
            Vector3 playerPosition = player.transform.position;

            PlayerSaveData playerData = new PlayerSaveData(playerPosition, player.namePlayer);

            SaveData saveData = new SaveData(backpack, money, toolbar, playerData);

            foreach (Vector3Int pos in tilemapManager.waterTilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemapManager.GetTile(tilemapManager.waterTilemap, pos);
                if (tile != null)
                {
                    TileType type = tilemapManager.GetTypeByTile(tile);
                    TileSaveData tileSaveData = new TileSaveData(pos, type);
                    saveData.waterTiles.Add(tileSaveData);
                }
            }

            foreach (Vector3Int pos in tilemapManager.landTilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemapManager.GetTile(tilemapManager.landTilemap, pos);
                if (tile != null)
                {
                    TileType type = tilemapManager.GetTypeByTile(tile);
                    TileSaveData tileSaveData = new TileSaveData(pos, type);
                    saveData.landTiles.Add(tileSaveData);
                }
            }

            foreach (Vector3Int pos in tilemapManager.groundObjectsTilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemapManager.GetTile(tilemapManager.groundObjectsTilemap, pos);
                if (tile != null)
                {
                    TileType type = tilemapManager.GetTypeByTile(tile);
                    TileSaveData tileSaveData = new TileSaveData(pos, type);
                    saveData.groundObjectsTiles.Add(tileSaveData);
                }
            }

          
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                if (obj.CompareTag("Tree") || obj.CompareTag("Bush") || obj.CompareTag("Flower") || obj.CompareTag("Shop") || obj.CompareTag("PlayerHome"))
                {
                    string specificType = obj.name; 
                    GameObjectSaveData objSaveData = new GameObjectSaveData(obj.transform.position, obj.tag, specificType);
                    saveData.gameObjectsData.Add(objSaveData);
                }
                else if(obj.CompareTag("GardenBed"))
                {
                    if(obj.TryGetComponent(out PlantHolder plantHolder))
                    {
                        if (plantHolder.plantableSteps != null)
                        {
                            ItemType type = plantHolder.plantableSteps.itemType;
                            bool isFertilized = plantHolder.isFertilized;
                            bool isReady = plantHolder.readyToHarvest;   
                            BedData bed = new BedData(obj.transform.position, type, isFertilized, isReady, plantHolder.stepIndex);
                            saveData.gardenBeds.Add(bed);
                        }
                        else
                        {
                            bool isFertilized = plantHolder.isFertilized;
                            bool isReady = plantHolder.readyToHarvest;

                            BedData bed = new BedData(obj.transform.position, ItemType.None, isFertilized, isReady, plantHolder.stepIndex);
                            saveData.gardenBeds.Add(bed);
                        }
                       
                    }
                    else
                    {
                        Debug.Log("No component PlantHolder!");
                    }
                }
            }

            saveManager.SaveGame(saveData);

            Debug.Log("Game saved.");
        }
        else
        {
            Debug.LogWarning("Data manager, player, or tile manager not found.");
        }
    }


    public void LoadGame()
    {
        if (saveManager != null && player != null)
        {
            SaveData saveData = saveManager.LoadGame();

            if (saveData != null)
            {

                ClearInventory(inventoryManager.GetInventoryByName("Backpack"));
                ClearInventory(inventoryManager.GetInventoryByName("Toolbar"));

                AddItemsToPlayerInventory(saveData.backpack, "Backpack");
                AddItemsToPlayerInventory(saveData.toolbar, "Toolbar");

                player.transform.position = saveData.playerData.position;
                player.SetName(saveData.playerData.playerName);

                moneyManager.ClearBalance();
                moneyManager.AddMoney(saveData.money);
                
                tilemapManager.ClearAllTilemaps();

                foreach (TileSaveData tileSaveData in saveData.waterTiles)
                {
                    tilemapManager.ApplyTileFromSaveData(tilemapManager.waterTilemap, tileSaveData);
                }

                foreach (TileSaveData tileSaveData in saveData.landTiles)
                {
                    tilemapManager.ApplyTileFromSaveData(tilemapManager.landTilemap, tileSaveData);
                }

                foreach (TileSaveData tileSaveData in saveData.groundObjectsTiles)
                {
                    tilemapManager.ApplyTileFromSaveData(tilemapManager.groundObjectsTilemap, tileSaveData);
                }

                ClearObjects();

                foreach (GameObjectSaveData objSaveData in saveData.gameObjectsData)
                {
                    GameObject prefab = GetPrefabByType(objSaveData.type, objSaveData.specificType);
                    if (prefab != null)
                    {
                        Instantiate(prefab, objSaveData.position, Quaternion.identity);
                    }
                }

                foreach (BedData bed in saveData.gardenBeds)
                {
                    GameObject bedObj = Instantiate(gardenBedPrefab, bed.position, Quaternion.identity);
                    
                    if (bedObj.TryGetComponent(out PlantHolder plantHolder))
                    {
                        Vector3Int pos = new Vector3Int((int)(bedObj.transform.position.x),
                        (int)(bedObj.transform.position.y),
                        0);
                        gardenManager.AddBed(pos, bedObj);
                        plantHolder.isFertilized = bed.isFertilized;
                        plantHolder.readyToHarvest = bed.isReady;
                        plantHolder.stepIndex = bed.stepIndex;

                        if (bed.itemType != ItemType.None && !bed.isReady)
                        {
                            PlantableSteps plantableSteps = plantStepsConfigurations.stepsConfigurations.FirstOrDefault(stepsConfig => stepsConfig.itemType == bed.itemType);

                            plantHolder.plantableSteps = plantableSteps;

                            StepData currentStep = plantableSteps.steps[plantHolder.stepIndex - 1];
                            plantHolder.spriteRenderer.sprite = currentStep.stepIcon;

                            plantHolder.StartGrow();
                        }
                        else if (bed.isReady)
                        {
                            PlantableSteps plantableSteps = plantStepsConfigurations.stepsConfigurations.FirstOrDefault(stepsConfig => stepsConfig.itemType == bed.itemType);

                            plantHolder.plantableSteps = plantableSteps;

                            StepData currentStep = plantableSteps.steps[plantHolder.stepIndex - 1];

                            plantHolder.spriteRenderer.sprite = currentStep.stepIcon;
                        }
                    }
                }
                GameManager.instance.uiManager.RefreshAll();

                Debug.Log("Game loaded.");
            }
            else
            {
                Debug.LogWarning("Failed to load game data.");
            }
        }
        else
        {
            Debug.LogWarning("Data manager or player not found.");
        }
    }

    private void ClearObjects()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.CompareTag("Tree") || obj.CompareTag("Bush") || obj.CompareTag("Flower") || obj.CompareTag("GardenBed") || obj.CompareTag("Shop") || obj.CompareTag("PlayerHome"))
            {
                Destroy(obj);
            }
        }
    }
    private GameObject GetPrefabByType(string type, string specificType)
    {
        switch (type)
        {
            case "Tree":
                return GetTreePrefabByName(specificType);
            case "Bush":
                return GetBushPrefabByName(specificType);
            case "Flower":
                return GetFlowerPrefabByName(specificType);
            case "GardenBed":
                return gardenBedPrefab;
            case "Shop":
                return shopPrefab;
            case "PlayerHome":
                return homePrefab;
            default:
                return null;
        }
    }

    private GameObject GetTreePrefabByName(string name)
    {
        switch (name)
        {
            case "AppleTree(Clone)":
                return appleTreePrefab;
            case "OakTree(Clone)":
                return oakTreePrefab;
            case "PineTree(Clone)":
                return pineTreePrefab;
            default:
                return null;
        }
    }

    private GameObject GetBushPrefabByName(string name)
    {
        switch (name)
        {
            case "bush1(Clone)":
                return bush1Prefab;
            case "bush2(Clone)":
                return bush2Prefab;
            default:
                return null;
        }
    }

    private GameObject GetFlowerPrefabByName(string name)
    {
        switch (name)
        {
            case "flower1(Clone)":
                return flower1Prefab;
            case "flower2(Clone)":
                return flower2Prefab;
            case "flower3(Clone)":
                return flower3Prefab;
            case "flower4(Clone)":
                return flower4Prefab;
            case "flower5(Clone)":
                return flower5Prefab;
            case "flower6(Clone)":
                return flower6Prefab;
            case "flower7(Clone)":
                return flower7Prefab;
            default:
                return null;
        }
    }

    private void AddItemsToPlayerInventory(Inventory inventory, string inventoryName)
    {
        if (inventory != null)
        {
            foreach (Inventory.Slot slot in inventory.slots)
            {
                if (slot != null && slot.itemData != null)
                {
                    Item item = GameManager.instance.itemManager.GetItemByName(slot.itemData.itemName);
                    if (item != null)
                    {
                        for (int i = 0; i < slot.count; i++)
                        {
                            inventoryManager.Add(inventoryName, item);
                        }
                    }
                }
            }
        }
    }

    private void ClearInventory(Inventory inventory)
    {
        if (inventory != null)
        {
            foreach (Inventory.Slot slot in inventory.slots)
            {
                while(slot.itemData != null)
                {
                    slot.RemoveItem();
                }
            }
        }
    }

}
