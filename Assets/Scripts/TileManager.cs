using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap tiles;

    [SerializeField] private Tile bedTile;


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
                if (tile is RuleTile)
                {
                    RuleTile ruleTile = tile as RuleTile;
                    return ruleTile.m_DefaultSprite != null ? ruleTile.m_DefaultSprite.name : "";
                }
                else
                {
                    // Если тайл не является RuleTile, возвращаем его имя
                    return tile.name;
                }
            }
        }

        return "";
    }

}
