using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public CollectableType type;
        public int count;
        public int maxAllowed;

        public Slot()
        {
            type = CollectableType.NONE; 
            count = 0;
            maxAllowed = 50;
        }

        public bool CanAddItem()
        {
            if(count < maxAllowed)
            {
                return true;
            }
            return false;
        }

        public void AddItem(CollectableType type)
        {
            this.type = type;
            count++;
        }
    }

    public List<Slot> slots = new List<Slot>();

    public Inventory(int numSlots)
    {
        for(int i = 0; i < numSlots; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void Add(CollectableType type)
    {
        foreach(Slot slot in slots)
        {
            if(slot.type == type && slot.CanAddItem())
            {
                slot.AddItem(type);
                return;
            }
        }

        foreach(Slot slot in slots)
        {
            if(slot.type == CollectableType.NONE)
            {
                slot.AddItem(type);
                return;
            }
        }
    }
}
