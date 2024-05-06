using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GardenManager : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> gardenBeds = new Dictionary<Vector3Int, GameObject>();

    public GameObject bedPrefab; // Префаб грядки

    public void CreateGardenBed(Vector3Int position)
    {
        // Проверяем, есть ли уже грядка на данной позиции
        if (!gardenBeds.ContainsKey(position))
        {
          
            Vector3 tileSize = new Vector3(1f, 1f, 0f); 

            Vector3 spawnPosition = position + tileSize / 2f;

            GameObject bed = Instantiate(bedPrefab, spawnPosition, Quaternion.identity);

            gardenBeds.Add(position, bed);
        }
    }

    public void RemoveGardenBed(Vector3Int position)
    {
        if (gardenBeds.ContainsKey(position))
        {
            Destroy(gardenBeds[position]);
            gardenBeds.Remove(position);
        }
    }
}