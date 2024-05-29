using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private int money;

    public event Action<int> onBalanceChanged;

    public void AddMoney(int amount)
    {
        money += amount;
        onBalanceChanged?.Invoke(money);
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        onBalanceChanged?.Invoke(money);
    }

    public int GetBalance()
    {
        return money;
    }

    public void ClearBalance()
    {
        money = 0;
    }
}
