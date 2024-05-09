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
            //Vector3 offset = new Vector3(0.5f, 0.5f, 0);

            //Vector3 spawnPos = position + offset;
            Debug.Log($"Key Position for this bed is {position}");

            Debug.Log($"Soawn Position for this bed is {position}");

            GameObject bed = Instantiate(bedPrefab, position, Quaternion.identity);

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