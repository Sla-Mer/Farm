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

    public Player player;

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

        player = FindObjectOfType<Player>();
    }

    public void SaveGame()
    {
        if (saveManager != null && player != null && tileManager != null)
        {
            // Получаем измененные тайлы и сохраняем игру
            List<TileData> modifiedTiles = tileManager.GetModifiedTiles();
            Inventory backpack = player.inventory.backpack; // Получаем рюкзак по имени
            Inventory toolbar = player.inventory.toolbar;  // Получаем тулбар по имени
            saveManager.SaveGame(modifiedTiles, backpack, toolbar); // Сохраняем рюкзак и тулбар
            Debug.Log("Backpack slots count" + backpack.slots.Count);
            Debug.Log("Toolbar slots count" + toolbar.slots.Count);
            Debug.Log("Backpack first slot item: " + backpack.slots[0].itemName);
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
            SaveData saveData = saveManager.LoadGame(); // Loading save data
            if (saveData != null)
            {
                // Loading inventory
                Inventory backpack = inventoryManager.GetInventoryByName("Backpack"); // Getting backpack
                Inventory toolbar = inventoryManager.GetInventoryByName("Toolbar"); // Getting toolbar
             
                Debug.Log("backpack slots " + backpack.slots.Count);
                Debug.Log("toolbar slots " + toolbar.slots.Count);

                backpack = backpack.CopyFrom(saveData.backpack);
                toolbar = toolbar.CopyFrom(saveData.toolbar);

                inventoryManager.backpack = backpack;
                inventoryManager.toolbar = toolbar;

                Debug.Log("First in backpack " + backpack.slots[0].itemName);

                Debug.Log("Inventory loaded.");

                // Apply changed tiles
                if (tileManager != null)
                {
                    tileManager.ApplyModifiedTiles(saveData.modifiedTiles);
                }
                else
                {
                    Debug.LogWarning("Tile manager not found.");
                }
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

}
