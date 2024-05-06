using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public Vector2Int position;
    public string tileType;

    public TileData(Vector2Int pos, string type)
    {
        position = pos;
        tileType = type;
    }
}

[System.Serializable]
public class SaveData
{
    public List<TileData> modifiedTiles = new List<TileData>();
    public Inventory backpack;
    public Inventory toolbar;

    public SaveData(List<TileData> tiles, Inventory backpack, Inventory toolbar)
    {
        modifiedTiles = tiles;
        this.backpack = backpack;
        this.toolbar = toolbar;
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
