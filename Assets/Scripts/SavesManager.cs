using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Water,
    Grass,
    Sand,
    Mountain,
    BlueFlower,
    WhiteFlower,
    Bed
}

[System.Serializable]
public class TileSaveData
{
    public Vector3Int position;
    public TileType type;

    public TileSaveData(Vector3Int position, TileType type)
    {
        this.position = position;
        this.type = type;
    }
}


[System.Serializable]
public class PlayerSaveData
{
    public Vector3 position;
    public string playerName;

    public PlayerSaveData(Vector3 position, string playerName)
    {
        this.position = position;
        this.playerName = playerName;
    }
}

[System.Serializable]
public class GameObjectSaveData
{
    public Vector3 position;
    public string type;
    public string specificType;

    public GameObjectSaveData(Vector3 position, string type, string specificType)
    {
        this.position = position;
        this.type = type;
        this.specificType = specificType;
    }
}

[System.Serializable]
public class BedData
{
    public Vector3 position;
    public ItemType itemType;
    public bool isFertilized;
    public bool isReady;
    public int stepIndex;

    public BedData(Vector3 position, ItemType itemType, bool isFertilized, bool isReady, int index)
    {
        this.position = position;
        this.itemType = itemType;
        this.isFertilized = isFertilized;
        this.isReady = isReady;
        this.stepIndex = index;
    }
}



[System.Serializable]
public class SaveData
{
    public Inventory backpack;
    public Inventory toolbar;
    public int money;
    public List<TileSaveData> waterTiles;
    public List<TileSaveData> landTiles;
    public List<TileSaveData> groundObjectsTiles;
    public PlayerSaveData playerData;
    public List<GameObjectSaveData> gameObjectsData;
    public List<BedData> gardenBeds;

    public SaveData(Inventory backpack, int money, Inventory toolbar, PlayerSaveData playerData)
    {
        this.backpack = backpack;
        this.toolbar = toolbar;
        this.money = money;
        this.playerData = playerData;
        waterTiles = new List<TileSaveData>();
        landTiles = new List<TileSaveData>();
        groundObjectsTiles = new List<TileSaveData>();
        gameObjectsData = new List<GameObjectSaveData>();
        gardenBeds = new List<BedData>();
    }
}


public class SavesManager : MonoBehaviour
{
    private static string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    public void SaveGame(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
    }

    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}
