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
}
