using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> gardenBeds = new Dictionary<Vector3Int, GameObject>();

    public GameObject bedPrefab;


    public void CreateGardenBed(Vector3Int position)
    {
        if (!gardenBeds.ContainsKey(position))
        {
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

    public void AddBed(Vector3Int position, GameObject bed)
    {
        if (!gardenBeds.ContainsKey(position))
        {
            gardenBeds.Add(position, bed);
        }
    }
}