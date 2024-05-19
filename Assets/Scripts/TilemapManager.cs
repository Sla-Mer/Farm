using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap waterTilemap;
    public Tilemap landTilemap;
    public Tilemap groundObjectsTilemap;

    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase sandTile;
    public TileBase mountainTile;
    public TileBase blueFlowerTile;
    public TileBase whiteFlowerTile;
    public TileBase bedTile;
    public void ApplyTile(Tilemap tilemap, Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
    }

    public TileBase GetTile(Tilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position);
    }

    public void ApplyTileFromSaveData(Tilemap tilemap, TileSaveData tileSaveData)
    {
        TileBase tileToApply = GetTileByType(tileSaveData.type);
        ApplyTile(tilemap, tileSaveData.position, tileToApply);
    }

    public TileSaveData GetSaveDataFromTile(Tilemap tilemap, Vector3Int position)
    {
        TileBase tile = GetTile(tilemap, position);
        TileType type = GetTypeByTile(tile);
        return new TileSaveData(position, type);
    }

    public TileBase GetTileByType(TileType type)
    {
        switch (type)
        {
            case TileType.Water:
                return waterTile;
            case TileType.Grass:
                return grassTile;
            case TileType.Sand:
                return sandTile;
            case TileType.Mountain:
                return mountainTile;
            case TileType.BlueFlower:
                return blueFlowerTile;
            case TileType.WhiteFlower:
                return whiteFlowerTile;
            case TileType.Bed:
                return bedTile;
            default:
                return null;
        }
    }

    public TileType GetTypeByTile(TileBase tile)
    {
        if (tile == waterTile)
            return TileType.Water;
        else if (tile == grassTile)
            return TileType.Grass;
        else if (tile == sandTile)
            return TileType.Sand;
        else if (tile == mountainTile)
            return TileType.Mountain;
        else if (tile == blueFlowerTile)
            return TileType.BlueFlower;
        else if (tile == whiteFlowerTile)
            return TileType.WhiteFlower;
        else if (tile == bedTile)
            return TileType.Bed;
        else
            return TileType.Grass; // Default to grass if tile not found
    }

    public void ClearTilemap(Tilemap tilemap)
    {
        tilemap.ClearAllTiles();
    }

    public void ClearAllTilemaps()
    {
        ClearTilemap(waterTilemap);
        ClearTilemap(landTilemap);
        ClearTilemap(groundObjectsTilemap);
    }
}
