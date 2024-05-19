using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public ItemData sellingItem;
    public SpriteRenderer itemSprite;
    public TextMeshProUGUI moneyText;

    private int price;
    public int multiplier;

    private void Awake()
    {
        itemSprite.sprite = sellingItem.icon;
        price = sellingItem.price * multiplier;
        moneyText.text = price.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.TryGetComponent(out Player player))
        {
            player.buyingItem = new ShopItemData(sellingItem, price);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.buyingItem = null;
        }
    }
}

public class ShopItemData
{
    public int price;
    public ItemData item;

    public ShopItemData(ItemData item, int price)
    {
        this.item = item;
        this.price = price;
    }
}
