using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;

    public TileManager tileManager;

    public SavesManager saveManager;

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

        // �������� ����������
        itemManager = GetComponent<ItemManager>();

        tileManager = GetComponent<TileManager>();

        saveManager = GetComponent<SavesManager>();
    }

    // ���������� ����
    public void SaveGame()
    {
        if (saveManager != null && player != null && tileManager != null)
        {
            // �������� ���������� ����� � ��������� ����
            List<TileData> modifiedTiles = tileManager.GetModifiedTiles();
            saveManager.SaveGame(modifiedTiles, player.inventory);
            Debug.Log("Game saved.");
        }
        else
        {
            Debug.LogWarning("Data manager, player, or tile manager not found.");
        }
    }

    // �������� ����
    public void LoadGame()
    {
        if (saveManager != null && player != null)
        {
            SaveData saveData = saveManager.LoadGame(); // ��������� ������ ����������
            if (saveData != null)
            {
                // ��������� ���������
                player.inventory = saveData.inventory;
                Debug.Log("Inventory loaded.");

                // ��������� ���������� �����
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
