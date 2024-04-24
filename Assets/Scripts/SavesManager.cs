using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public Vector2Int position;
    public int tileType;

    public TileData(Vector2Int pos, int type)
    {
        position = pos;
        tileType = type;
    }
}

[System.Serializable]
public class SaveData
{
    public List<TileData> modifiedTiles = new List<TileData>();
    public Inventory inventory;

    public SaveData(List<TileData> tiles, Inventory inv)
    {
        modifiedTiles = tiles;
        inventory = inv;
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

    public void SaveGame(List<TileData> modifiedTiles, Inventory inventory)
    {
        SaveData saveData = new SaveData(modifiedTiles, inventory);
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
