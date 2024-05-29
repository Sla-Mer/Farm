using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

[Serializable]
public class GameData
{
    public string PlayerName;
    public string WorldSeed;
    public bool IsNewGame;

    public GameData(string playerName, string worldSeed, bool isNewGame)
    {
        PlayerName = playerName;
        WorldSeed = worldSeed;
        IsNewGame = isNewGame;
    }

    public GameData(bool isNewGame)
    {
        IsNewGame = isNewGame;
    }
}

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/gamedata.json";

    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            return data;
        }
        return null;
    }
}