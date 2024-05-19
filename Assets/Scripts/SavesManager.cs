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
public class SaveData
{
    public Inventory backpack;
    public Inventory toolbar;
    public int money;
    public List<TileSaveData> waterTiles;
    public List<TileSaveData> landTiles;
    public List<TileSaveData> groundObjectsTiles;

    public SaveData(Inventory backpack,int money, Inventory toolbar)
    {
        this.backpack = backpack;
        this.toolbar = toolbar;
        this.money = money;
        waterTiles = new List<TileSaveData>();
        landTiles = new List<TileSaveData>();
        groundObjectsTiles = new List<TileSaveData>();
    }
}

public class SavesManager : MonoBehaviour
{
    public static SavesManager instance;

    private string savePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        savePath = "Saves/save.json";
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
