using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GardenManager : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> gardenBeds = new Dictionary<Vector3Int, GameObject>();

    public GameObject bedPrefab; 

    public void CreateGardenBed(Vector3Int position)
    {
        // ѕровер€ем, есть ли уже гр€дка на данной позиции
        if (!gardenBeds.ContainsKey(position))
        {
            Debug.Log($"Key Position for this bed is {position}");
          
            Vector3 tileSize = new Vector3(1f, 1f, 0f); 

            Vector3 spawnPosition = position + tileSize / 2f;

            Debug.Log($"Soawn Position for this bed is {spawnPosition}");

            GameObject bed = Instantiate(bedPrefab, spawnPosition, Quaternion.identity);

            gardenBeds.Add(position, bed);
        }
    }

    public void RemoveGardenBed(Vector3Int position)
    {
        Debug.Log($"Position for destroy: {position}");
        Destroy(gardenBeds[position]);
        gardenBeds.Remove(position);
    }
}