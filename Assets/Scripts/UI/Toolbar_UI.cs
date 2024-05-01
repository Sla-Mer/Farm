using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    public int toolbarSlotsCount;

    private void Start()
    {
        SelectSlot(0);
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
    }
    public void SelectSlot(int index)
    {
        if(toolbarSlots.Count == toolbarSlotsCount)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }
            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);

            GameManager.instance.inventoryManager.toolbar.SelectSlot(index);
        }
    }

    private void CheckAlphaNumericKeys()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        { 
            SelectSlot(0);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }

        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }

        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }

        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            SelectSlot(8);
        }
    }
}
