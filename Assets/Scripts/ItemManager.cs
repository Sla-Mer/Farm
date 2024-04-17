using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Collectable[] collectableItems;

    private Dictionary<CollectableType, Collectable> collectableitemsDict = new Dictionary<CollectableType, Collectable>();

    private void Awake()
    {
        foreach (Collectable collectable in collectableItems)
        {
            AddItem(collectable);
        }
    }

    private void AddItem(Collectable collectable)
    {
        if(!collectableitemsDict.ContainsKey(collectable.type))
        {
            collectableitemsDict.Add(collectable.type, collectable);
        }
    }

    public Collectable GetItemByType(CollectableType type)
    {
        if (collectableitemsDict.ContainsKey(type))
        {
            return collectableitemsDict[type];
        }

        return null;
    }
}
