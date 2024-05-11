using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public int width = 500;
    public int height = 500;
    public float scale = 20f;
    public int seed = 248;
    public Tilemap waterTilemap;
    public Tilemap landTilemap;
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase sandTile;

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        Random.InitState(seed); // Инициализация генератора случайных чисел с помощью сида

        // Перебор всех пикселей в мире
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                // Расстояние от текущей точки до центра мира
                float distanceToCenter = Vector2.Distance(new Vector2(x, y), Vector2.zero);

                // Если расстояние больше радиуса острова, считаем это за воду
                if (distanceToCenter >= width / 2)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
                else
                {
                    // Генерация значения шума Перлина с учетом сида
                    float noiseValue = Mathf.PerlinNoise((float)x / width * scale + seed, (float)y / height * scale + seed);

                    // Устанавливаем тайл земли на соответствующее место в Tilemap Land
                    if (noiseValue < 0.6f)
                    {
                        landTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                    }
                    else
                    {
                        landTilemap.SetTile(new Vector3Int(x, y, 0), sandTile);
                    }
                }
            }
        }
    }
}
