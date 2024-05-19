using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Item data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName = "Item Name";
    public Sprite icon;
    public bool isPlantable = false;
    public bool isSellable = false;
    [Min(0)]public int price = 0;
}
