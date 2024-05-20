using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;

    public TileManager tileManager;

    public TilemapManager tilemapManager;

    public SavesManager saveManager;

    public InventoryManager inventoryManager;

    public UI_Manager uiManager;

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

    public GameObject shopPrefab;

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

        tilemapManager = GetComponent<TilemapManager>();

        saveManager = GetComponent<SavesManager>();

        inventoryManager = GetComponent<InventoryManager>();

        uiManager = GetComponent<UI_Manager>();

        gardenManager = GetComponent<GardenManager>();

        menuController = GetComponent<MenuController>();

        worldGenerator = GetComponent<WorldGenerator>();

        moneyManager = GetComponent<MoneyManager>();

        player = FindObjectOfType<Player>();

        moneyManager.AddMoney(2000);
    }

    public void SaveGame()
    {
        if (saveManager != null && player != null && tileManager != null)
        {
            Inventory backpack = GameManager.instance.inventoryManager.GetInventoryByName("Backpack");
            Inventory toolbar = GameManager.instance.inventoryManager.GetInventoryByName("Toolbar");

            int money = moneyManager.GetBalance();
            Vector3 playerPosition = player.transform.position;
            PlayerSaveData playerData = new PlayerSaveData(playerPosition);

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

            // ���������� ������� ��������
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                // ���������� ��������, ������� ����� ���������, ��������, �������, �����, ������
                if (obj.CompareTag("Tree") || obj.CompareTag("Bush") || obj.CompareTag("GardenBed") || obj.CompareTag("Shop"))
                {
                    string specificType = obj.name; // ���������� ��� ������� ��� ���������� ������������� ����
                    GameObjectSaveData objSaveData = new GameObjectSaveData(obj.transform.position, obj.tag, specificType);
                    saveData.gameObjectsData.Add(objSaveData);
                }
            }

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
                AddItemsToPlayerInventory(saveData.backpack, "Backpack");
                AddItemsToPlayerInventory(saveData.toolbar, "Toolbar");

                // �������������� ������� ������
                player.transform.position = saveData.playerData.position;

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

                // �������������� ������� ��������
                foreach (GameObjectSaveData objSaveData in saveData.gameObjectsData)
                {
                    GameObject prefab = GetPrefabByType(objSaveData.type, objSaveData.specificType);
                    if (prefab != null)
                    {
                        Instantiate(prefab, objSaveData.position, Quaternion.identity);
                    }
                }

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

    private void ClearObjects()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            // ���������� ��������, ������� ����� ���������, ��������, �������, �����, ������
            if (obj.CompareTag("Tree") || obj.CompareTag("Bush") || obj.CompareTag("GardenBed") || obj.CompareTag("Shop"))
            {
                Destroy(obj);
            }
        }
    }
    private GameObject GetPrefabByType(string type, string specificType)
    {
        // ����� ��� ��������� ������� �� ���� � ����������� ���� (specificType)
        switch (type)
        {
            case "Tree":
                return GetTreePrefabByName(specificType);
            case "Bush":
                return GetBushPrefabByName(specificType);
            case "GardenBed":
                return gardenBedPrefab;
            case "Shop":
                return shopPrefab;
            default:
                return null;
        }
    }

    private GameObject GetTreePrefabByName(string name)
    {
        // ���������� ������ ��� �������� ������� ������ �� �����
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
        // ���������� ������ ��� �������� ������� ����� �� �����
        switch (name)
        {
            case "bush1(Clone)":
                return bush1Prefab;
            case "bush2(Clone)":
                return bush2Prefab;
            // �������� ������ ���� ������
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
                            player.inventoryManager.Add(inventoryName, item);
                        }
                    }
                }
            }
        }
    }

}
