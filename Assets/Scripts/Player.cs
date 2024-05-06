using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    public InventoryManager inventoryManager;

    private TileManager tileManager;

    public GameObject bedPrefab;

    private GardenManager gardenManager;

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
        gardenManager = GameManager.instance.gardenManager;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Vector3Int pos = new Vector3Int((int)transform.position.x,
                (int)transform.position.y,
                0);

            string tileName = tileManager.GetTileName(pos);

            if (!string.IsNullOrWhiteSpace(tileName))
            {
                if (tileName == "tiles_306" && inventoryManager.toolbar.selectedSlot.itemName == "Hoe")
                {
                    tileManager.SetInteracted(pos);
                }
                else if (tileName == "tiles_451" && inventoryManager.toolbar.selectedSlot.itemName == "Shovel")
                {
                    // Создаем грядку с использованием менеджера грядок
                    gardenManager.CreateGardenBed(pos);
                }
            }
        }
       
    }

    private void CreateBed(Vector3Int pos)
    {
        // Размер тайла в мировых координатах
        Vector3 tileSize = new Vector3(1f, 1f, 0f); // Предполагается, что размер тайла 1x1

        // Позиция для размещения грядки в центре тайла
        Vector3 spawnPosition = pos + tileSize / 2f;

        // Создание экземпляра префаба грядки
        GameObject bed = Instantiate(bedPrefab, spawnPosition, Quaternion.identity);

    }
    public void DropItem(Item item)
    {
        Vector2 spawnPosition = transform.position;

        float radius = 1.5f;

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        Vector2 randomOnCircle = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * radius;

        Item droppedItem = Instantiate(item, spawnPosition + randomOnCircle, Quaternion.identity);

        droppedItem.rb2d.AddForce(randomOnCircle.normalized * 0.5f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
