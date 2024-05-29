using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Tilemap groundObjectsTilemap;
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase sandTile;
    public TileBase mountainTile;
    public TileBase blueFlowerTile;
    public TileBase whiteFlowerTile;
    public GameObject[] treePrefabs;
    public GameObject[] bushPrefabs;
    public GameObject[] flowerPrefabs;
    public GameObject shopPrefab;
    public float flowerSpawnChance = 5f;
    public float treeSpawnChance = 50f;
    public float bushSpawnChance = 25f;
    public float flowerObjectSpawnChance = 15f;
    private List<Vector3Int> treePositions = new List<Vector3Int>();
    private List<Vector3Int> bushPositions = new List<Vector3Int>();
    private List<Vector3Int> flowerPositions = new List<Vector3Int>();

    void Start()
    {
        GameData gameData = SaveSystem.LoadGame();

        if (gameData != null)
        {
            if(gameData.IsNewGame)
            {
                seed = gameData.WorldSeed;
                GenerateWorld();
                PlaceTrees();
                PlaceBushes();
                PlaceFlowers();
                PlaceShop();
                GiveStartInventory();

                GameManager.instance.player.SetName(gameData.PlayerName);

                GameManager.instance.SaveGame();
            }
        }
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

                TrySpawnTree(x, y);
                TrySpawnBush(x, y);
                TrySpawnFlowerPrefab(x, y);
            }
        }
    }

    void TrySpawnFlower(int x, int y)
    {
        float randomChance = Random.Range(0, 1000);
        if (randomChance < flowerSpawnChance)
        {
            TileBase flowerTile = Random.Range(0, 2) == 0 ? blueFlowerTile : whiteFlowerTile;
            landTilemap.SetTile(new Vector3Int(x, y, 0), flowerTile);
        }
    }

    void PlaceTrees()
    {
        foreach (Vector3Int pos in treePositions)
        {
            int randomIndex = Random.Range(0, treePrefabs.Length);
            Instantiate(treePrefabs[randomIndex], groundObjectsTilemap.GetCellCenterWorld(pos), Quaternion.identity);
        }
    }

    void TrySpawnTree(int x, int y)
    {
        float randomChance = Random.Range(0, 1000);
        TileBase tile = landTilemap.GetTile(new Vector3Int(x, y, 0));
        if (randomChance < treeSpawnChance && tile == grassTile && waterTilemap.GetTile(new Vector3Int(x, y, 0)) == null)
        {
            bool canSpawn = true;
            foreach (Vector3Int treePos in treePositions)
            {
                float distance = Vector3Int.Distance(treePos, new Vector3Int(x, y, 0));
                if (distance < 2)
                {
                    canSpawn = false;
                    break;
                }
            }
            if (canSpawn)
            {
                treePositions.Add(new Vector3Int(x, y, 0));
            }
        }
    }

    void PlaceBushes()
    {
        foreach (Vector3Int pos in bushPositions)
        {
            int randomIndex = Random.Range(0, bushPrefabs.Length);
            Instantiate(bushPrefabs[randomIndex], groundObjectsTilemap.GetCellCenterWorld(pos), Quaternion.identity);
        }
    }

    void TrySpawnBush(int x, int y)
    {
        float randomChance = Random.Range(0, 1000);
        TileBase tile = landTilemap.GetTile(new Vector3Int(x, y, 0));
        if (randomChance < bushSpawnChance && tile == grassTile && waterTilemap.GetTile(new Vector3Int(x, y, 0)) == null)
        {
            bool canSpawn = true;
            foreach (Vector3Int bushPos in bushPositions)
            {
                float distance = Vector3Int.Distance(bushPos, new Vector3Int(x, y, 0));
                if (distance < 2)
                {
                    canSpawn = false;
                    break;
                }
            }
            if (canSpawn)
            {
                bushPositions.Add(new Vector3Int(x, y, 0));
            }
        }
    }

    void PlaceFlowers()
    {
        foreach (Vector3Int pos in flowerPositions)
        {
            int randomIndex = Random.Range(0, flowerPrefabs.Length);
            Instantiate(flowerPrefabs[randomIndex], groundObjectsTilemap.GetCellCenterWorld(pos), Quaternion.identity);
        }
    }

    void TrySpawnFlowerPrefab(int x, int y)
    {
        float randomChance = Random.Range(0, 1000);
        TileBase tile = landTilemap.GetTile(new Vector3Int(x, y, 0));
        if (randomChance < flowerObjectSpawnChance && tile == grassTile && waterTilemap.GetTile(new Vector3Int(x, y, 0)) == null)
        {
            bool canSpawn = true;
            foreach (Vector3Int flowerPos in flowerPositions)
            {
                float distance = Vector3Int.Distance(flowerPos, new Vector3Int(x, y, 0));
                if (distance < 2)
                {
                    canSpawn = false;
                    break;
                }
            }
            if (canSpawn)
            {
                flowerPositions.Add(new Vector3Int(x, y, 0));
            }
        }
    }
    void PlaceShop()
    {
        Vector3Int shopPosition = Vector3Int.zero;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (IsValidShopPosition(position))
                {
                    shopPosition = position;
                    break;
                }
            }
            if (shopPosition != Vector3Int.zero)
                break;
        }

        if (shopPosition != Vector3Int.zero)
        {
            ClearArea(shopPosition, 10);
            Instantiate(shopPrefab, groundObjectsTilemap.GetCellCenterWorld(shopPosition), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Unable to find a suitable location for the shop.");
        }
    }

    bool IsValidShopPosition(Vector3Int position)
    {
        if (landTilemap.GetTile(position) != grassTile || waterTilemap.GetTile(position) != null)
            return false;

        int radius = 3;
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                Vector3Int checkPosition = new Vector3Int(position.x + dx, position.y + dy, 0);
                if (landTilemap.GetTile(checkPosition) != grassTile)
                    return false;
            }
        }

        // ƒополнительна€ проверка на рассто€ние до центра
        float distanceToOrigin = Vector2.Distance(Vector2.zero, new Vector2(position.x, position.y));
        if (distanceToOrigin > 100)
            return false;

        return true;
    }

    void ClearArea(Vector3Int center, int radius)
    {
        Vector3 centerWorldPos = groundObjectsTilemap.GetCellCenterWorld(center);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(centerWorldPos, radius * groundObjectsTilemap.cellSize.x);

        foreach (Collider2D collider in colliders)
        {
            Destroy(collider.gameObject);
        }
    }

    public void GiveStartInventory()
    {
        InventoryManager inventory = GameManager.instance.inventoryManager;
        inventory.Add("Backpack", GameManager.instance.itemManager.GetItemByName("Hoe"));
        inventory.Add("Backpack", GameManager.instance.itemManager.GetItemByName("Shovel"));
        for (int i = 0; i < 4; i++)
        {
            inventory.Add("Backpack", GameManager.instance.itemManager.GetItemByName("Carrot Seeds"));
        }

        GameManager.instance.player.SetName("Farmer");

        GameManager.instance.moneyManager.AddMoney(50);
    }
}
