using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap tiles;

    [SerializeField] private Tile bedTile;
    void Start()
    {
        
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = tiles.GetTile(position);

        if(tile != null)
        {
            if(tile.name == "tiles_306")
            {
                return true;
            }
        }
        return false;
    }

    public void SetInteracted(Vector3Int position)
    {
        tiles.SetTile(position, bedTile);
    }


    public List<TileData> GetModifiedTiles()
    {
        List<TileData> modifiedTiles = new List<TileData>();

        BoundsInt bounds = tiles.cellBounds;
        TileBase[] allTiles = tiles.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = tiles.GetTile(pos);
                if (tile != null)
                {
                    // Проверяем, изменился ли тайл с момента последнего сохранения
                    if (IsTileModified(pos, tile))
                    {
                        modifiedTiles.Add(new TileData(new Vector2Int(x, y), GetTileType(tile)));
                    }
                }
            }
        }

        return modifiedTiles;
    }

    // Метод для проверки, изменился ли тайл
    private bool IsTileModified(Vector3Int position, TileBase tile)
    {
        // Здесь вы можете реализовать свою логику для проверки изменений тайла
        // Например, вы можете хранить предыдущее состояние тайлов и сравнивать их с текущим состоянием
        // В этом примере я просто считаю тайл измененным, если его тип является GrassTile

        if (tile.name == "tiles_451") // Замените на свой тип тайла, если он отличается
        {
            // Здесь можно выполнить дополнительные проверки на изменение тайла, если необходимо
            return true;
        }

        return false;
    }

    // Метод для получения типа тайла
    private int GetTileType(TileBase tile)
    {
        // Здесь вы можете определить тип тайла
        // Например, вы можете присвоить каждому типу тайла уникальный идентификатор
        // В этом примере я просто возвращаю 0 для GrassTile

        if (tile.name == "tiles_451") // Замените на свой тип тайла, если он отличается
        {
            return 451; // Пример типа для GrassTile
        }

        return 306; // Если тип неизвестен или не определен
    }
    public void ApplyModifiedTiles(List<TileData> modifiedTiles)
    {
        if (tiles != null && modifiedTiles != null)
        {
            foreach (TileData tileData in modifiedTiles)
            {
                Vector3Int tilePosition = new Vector3Int(tileData.position.x, tileData.position.y, 0);
                tiles.SetTile(tilePosition, bedTile); 

            }
        }
        else
        {
            Debug.LogWarning("Tilemap or modified tiles list is null.");
        }
    }

}
