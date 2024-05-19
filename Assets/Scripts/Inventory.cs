using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;
using static UnityEditor.Progress;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public ItemData itemData;
        public int count;
        public int maxAllowed;

        public Slot()
        {
            itemData = null; 
            count = 0;
            maxAllowed = 50;
        }

        public bool IsEmpty
        {
            get
            {
                if(itemData == null && count == 0)
                {
                    return true;
                }

                return false;
            }
        }
        public bool CanAddItem(string itemName)
        {
            if(this.itemData != null && this.itemData.itemName == itemName && count < maxAllowed)
            {
                return true;
            }
            return false;
        }

        public void AddItem(Item item)
        {
            this.itemData = item.data;
            count++;
        }

        public void AddItem(ItemData itemData, int maxAllowed)
        {
            this.itemData = itemData;
            count++;
            this.maxAllowed = maxAllowed;
        }

        public void RemoveItem()
        {
            if(count > 0)
            {
                count --;
                
                if(count == 0)
                {
                    itemData = null;
                }
            }
        }
    }


    public List<Slot> slots = new List<Slot>();

    public Slot selectedSlot = null;

    public Inventory(int numSlots)
    {
        for(int i = 0; i < numSlots; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void Add(Item item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemData == null)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Add(ItemData itemData)
    {
        foreach (Slot slot in slots)
        {
            if (slot.CanAddItem(itemData.itemName))
            {
                slot.AddItem(itemData, slot.maxAllowed);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemData == null)
            {
                slot.AddItem(itemData, slot.maxAllowed);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }

    public void Remove(int index, int numToRemove)
    {
        if (slots[index].count >= numToRemove)
        {
            for(int i = 0;i < numToRemove; i++)
            {
                Remove(index);
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if(toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemData.itemName))
        {
            for(int i = 0; i < numToMove; i++)
            {
                toSlot.AddItem(fromSlot.itemData, fromSlot.maxAllowed);
                fromSlot.RemoveItem();
            }
        }
    }

    public void SelectSlot(int index)
    {
        if(slots != null && slots.Count > 0){
            selectedSlot = slots[index];
        }
    }
}
