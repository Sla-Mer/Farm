using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public int width = 500;
    public int height = 500;
    public float scale = 0.5f;
    public Tilemap waterTilemap;
    public Tilemap landTilemap;
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase SandTile;

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        // Перебор всех пикселей в мире
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Расстояние от текущей точки до центра мира
                float distanceToCenter = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, height / 2));

                // Если расстояние больше радиуса острова, считаем это за воду
                if (distanceToCenter >= width / 2)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
                else
                {
                    // Генерация значения шума Перлина
                    float noiseValue = Mathf.PerlinNoise((float)x / width * scale, (float)y / height * scale);

                    // Устанавливаем тайл земли на соответствующее место в Tilemap Land
                    if (noiseValue < 0.6f)
                    {
                        landTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                    }
                    else
                    {
                        landTilemap.SetTile(new Vector3Int(x, y, 0), SandTile);
                    }
                }
            }
        }
    }
}
