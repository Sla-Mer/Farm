using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float scale;
    public int seed;
    public Tilemap waterTilemap;
    public Tilemap landTilemap;
    public Tilemap groundObjectsTilemap; // Новый Tilemap для объектов на земле
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase sandTile;
    public TileBase mountainTile; // Новый тайл для гор
    public TileBase blueFlowerTile;
    public TileBase whiteFlowerTile;
    public float flowerSpawnChance = 5f; // Шанс появления цветов

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        Random.InitState(seed);

        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                float distanceToCenter = Vector2.Distance(new Vector2(x, y), Vector2.zero);
                float noiseValue = Mathf.PerlinNoise((float)x / width * scale + seed, (float)y / height * scale + seed);

                if (distanceToCenter >= width / 2)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
                else if (noiseValue < 0.15f)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }

                else if (noiseValue >= 0.15f && noiseValue < 0.4f)
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                    TrySpawnFlower(x, y);
                }

                else if (noiseValue >= 0.4f && noiseValue < 0.5f)
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), sandTile);
                }

                else if (noiseValue >= 0.5f && noiseValue < 0.8f)
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                    TrySpawnFlower(x, y);
                }
                else if (noiseValue >= 0.8f && noiseValue < 1.1f)
                {
                    groundObjectsTilemap.SetTile(new Vector3Int(x, y, 0), mountainTile);
                    landTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                }


            }
        }
    }

    void TrySpawnFlower(int x, int y)
    {
        float randomChance = Random.Range(0,1000);
        if (randomChance < flowerSpawnChance)
        {
            TileBase flowerTile = Random.Range(0, 2) == 0 ? blueFlowerTile : whiteFlowerTile;
            landTilemap.SetTile(new Vector3Int(x, y, 0), flowerTile);
        }
    }
}
