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
                    // ���������, ��������� �� ���� � ������� ���������� ����������
                    if (IsTileModified(pos, tile))
                    {
                        modifiedTiles.Add(new TileData(new Vector2Int(x, y), GetTileType(tile)));
                    }
                }
            }
        }

        return modifiedTiles;
    }

    // ����� ��� ��������, ��������� �� ����
    private bool IsTileModified(Vector3Int position, TileBase tile)
    {
        // ����� �� ������ ����������� ���� ������ ��� �������� ��������� �����
        // ��������, �� ������ ������� ���������� ��������� ������ � ���������� �� � ������� ����������
        // � ���� ������� � ������ ������ ���� ����������, ���� ��� ��� �������� GrassTile

        if (tile.name == "tiles_451") // �������� �� ���� ��� �����, ���� �� ����������
        {
            // ����� ����� ��������� �������������� �������� �� ��������� �����, ���� ����������
            return true;
        }

        return false;
    }

    // ����� ��� ��������� ���� �����
    private int GetTileType(TileBase tile)
    {
        // ����� �� ������ ���������� ��� �����
        // ��������, �� ������ ��������� ������� ���� ����� ���������� �������������
        // � ���� ������� � ������ ��������� 0 ��� GrassTile

        if (tile.name == "tiles_451") // �������� �� ���� ��� �����, ���� �� ����������
        {
            return 451; // ������ ���� ��� GrassTile
        }

        return 306; // ���� ��� ���������� ��� �� ���������
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
