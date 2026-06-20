using System;
using UnityEngine;

[Serializable]
public class ShopStockEntry
{
    public ItemData item;
    public int priceOverride = -1;

    public int GetBuyPrice()
    {
        if (item == null)
            return 0;

        return priceOverride >= 0 ? priceOverride : item.value;
    }
}