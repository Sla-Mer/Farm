using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;

    public TileManager tileManager;

    public SavesManager saveManager;

    public InventoryManager inventoryManager;

    public UI_Manager uiManager;

    public GardenManager gardenManager;

    public Player player;

    public MenuController menuController;

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

        DontDestroyOnLoad(this.gameObject);

        // �������� ����������
        itemManager = GetComponent<ItemManager>();

        tileManager = GetComponent<TileManager>();

        saveManager = GetComponent<SavesManager>();

        inventoryManager = GetComponent<InventoryManager>();

        uiManager = GetComponent<UI_Manager>();

        gardenManager = GetComponent<GardenManager>();

        menuController = GetComponent<MenuController>();

        player = FindObjectOfType<Player>();
    }

    public void SaveGame()
    {
        if (saveManager != null && player != null && tileManager != null)
        {
            // �������� ���������� �����
            List<TileData> modifiedTiles = tileManager.GetModifiedTiles();

            // �������� ��������� �� InventoryManager
            Inventory backpack = GameManager.instance.inventoryManager.GetInventoryByName("Backpack");
            Inventory toolbar = GameManager.instance.inventoryManager.GetInventoryByName("Toolbar");

            // ������� ������ SaveData � �������� � ���� ������
            SaveData saveData = new SaveData(modifiedTiles, backpack, toolbar);

            // ��������� ����
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
            // ��������� ������ ����������
            SaveData saveData = saveManager.LoadGame();

            if (saveData != null)
            {
                // ��������� ������ �� ���������� � ����������
                GameManager.instance.inventoryManager.backpack = saveData.backpack;
                GameManager.instance.inventoryManager.toolbar = saveData.toolbar;

                // ��������� ���������� �����
                if (tileManager != null)
                {
                    tileManager.ApplyModifiedTiles(saveData.modifiedTiles);
                }
                else
                {
                    Debug.LogWarning("Tile manager not found.");
                }

                // ��������� �������� �� ���������� � ��������� ������
                AddItemsToPlayerInventory(saveData.backpack, "Backpack");
                AddItemsToPlayerInventory(saveData.toolbar, "Toolbar");

                // ��������� UI
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
                            player.inventoryManager.Add(inventoryName, item);
                        }
                    }
                }
            }
        }
    }

}
