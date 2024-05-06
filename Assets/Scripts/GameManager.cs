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

        // Получаем компоненты
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
            // Получаем измененные тайлы
            List<TileData> modifiedTiles = tileManager.GetModifiedTiles();

            // Получаем инвентари из InventoryManager
            Inventory backpack = GameManager.instance.inventoryManager.GetInventoryByName("Backpack");
            Inventory toolbar = GameManager.instance.inventoryManager.GetInventoryByName("Toolbar");

            // Создаем объект SaveData и передаем в него данные
            SaveData saveData = new SaveData(modifiedTiles, backpack, toolbar);

            // Сохраняем игру
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
            // Загружаем данные сохранения
            SaveData saveData = saveManager.LoadGame();

            if (saveData != null)
            {
                // Применяем данные из сохранения к инвентарям
                GameManager.instance.inventoryManager.backpack = saveData.backpack;
                GameManager.instance.inventoryManager.toolbar = saveData.toolbar;

                // Применяем измененные тайлы
                if (tileManager != null)
                {
                    tileManager.ApplyModifiedTiles(saveData.modifiedTiles);
                }
                else
                {
                    Debug.LogWarning("Tile manager not found.");
                }

                // Добавляем предметы из сохранения в инвентарь игрока
                AddItemsToPlayerInventory(saveData.backpack, "Backpack");
                AddItemsToPlayerInventory(saveData.toolbar, "Toolbar");

                // Обновляем UI
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
