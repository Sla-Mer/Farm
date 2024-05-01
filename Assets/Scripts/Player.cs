using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventoryManager;

    private TileManager tileManager;

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Vector3Int pos = new Vector3Int((int)transform.position.x,
                (int)transform.position.y,
                0);

            string tileName = tileManager.GetTileName(pos);

            if (!string.IsNullOrWhiteSpace(tileName))
            {
                if(tileName == "tiles_306" && inventoryManager.toolbar.selectedSlot.itemName == "Hoe")
                {
                    tileManager.SetInteracted(pos);
                }
            }
        }
    }
    public void DropItem(Item item)
    {
        Vector2 spawnPosition = transform.position;

        float radius = 1.5f;

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        Vector2 randomOnCircle = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * radius;

        Item droppedItem = Instantiate(item, spawnPosition + randomOnCircle, Quaternion.identity);

        droppedItem.rb2d.AddForce(randomOnCircle.normalized * 0.5f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
