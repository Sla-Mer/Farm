using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    public GameObject inventoryPanel;

    public TextMeshProUGUI moneyText;

    public TextMeshProUGUI playerNameText;

    public List<Inventory_UI> inventoryUIs;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    private void OnEnable()
    {
        GameManager.instance.moneyManager.onBalanceChanged += UpdateMoneyLabel;
    }

    private void OnDisable()
    {
        GameManager.instance.moneyManager.onBalanceChanged -= UpdateMoneyLabel;
    }


    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleInventoryUI();
        }
        dragSingle = Input.GetKey(KeyCode.LeftShift);
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll()
    {
        foreach(KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }
    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }
        Debug.Log("No UI for the " + inventoryName);
        return null;
    }

    private void Initialize()
    {
        foreach (Inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
        UpdateMoneyLabel(GameManager.instance.moneyManager.GetBalance());
    }


    public void ToggleInventoryUI()
    {
        if (inventoryPanel != null)
        {
            if (!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else
            {
                inventoryPanel.SetActive(false);
            }
        }
    }

    private void UpdateMoneyLabel(int amount)
    {
        moneyText.text = amount.ToString();
    }

    public void UpdatePlayerName()
    {
        playerNameText.text = GameManager.instance.player.namePlayer;
    }
}
