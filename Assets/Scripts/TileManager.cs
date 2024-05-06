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

    public void SetInteracted(Vector3Int position)
    {
        tiles.SetTile(position, bedTile);
    }

    public string GetTileName(Vector3Int position)
    {
        if (tiles != null)
        {
            TileBase tile = tiles.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }

        return "";
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
                    if (IsTileModified(pos, tile))
                    {
                        modifiedTiles.Add(new TileData(new Vector2Int(x, y), GetTileType(tile)));
                    }
                }
            }
        }

        return modifiedTiles;
    }

    private bool IsTileModified(Vector3Int position, TileBase tile)
    {

        if (tile.name == "tiles_451")
        {
            return true;
        }

        return false;
    }

    private string GetTileType(TileBase tile)
    {

        if (tile.name == "tiles_451") 
        {
            return "tiles_451"; 
        }

        return "tiles_306"; 
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
