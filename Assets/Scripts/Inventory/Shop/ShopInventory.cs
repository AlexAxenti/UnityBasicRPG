using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    [Header("Base Stock")]
    [SerializeField] private List<ShopStockEntry> baseStock = new();

    [Header("Resale Stock")]
    [SerializeField] private List<InventorySlot> resaleStock = new();

    public IReadOnlyList<ShopStockEntry> BaseStock => baseStock;
    public IReadOnlyList<InventorySlot> ResaleStock => resaleStock;

    public void AddResaleItem(ItemData item, int quantity)
    {
        if (item == null || quantity <= 0)
            return;

        if (item.maxStack > 1)
        {
            for (int i = 0; i < resaleStock.Count; i++)
            {
                if (resaleStock[i].item == item && resaleStock[i].quantity < item.maxStack)
                {
                    int space = item.maxStack - resaleStock[i].quantity;
                    int toAdd = Mathf.Min(space, quantity);

                    resaleStock[i].quantity += toAdd;
                    quantity -= toAdd;

                    if (quantity <= 0)
                        return;
                }
            }
        }

        while (quantity > 0)
        {
            int stackAmount = Mathf.Min(item.maxStack, quantity);
            resaleStock.Add(new InventorySlot(item, stackAmount));
            quantity -= stackAmount;
        }
    }

    public bool RemoveResaleItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= resaleStock.Count)
            return false;

        resaleStock.RemoveAt(slotIndex);
        return true;
    }

    public List<InventorySlot> BuildBaseStockDisplaySlots()
    {
        List<InventorySlot> displaySlots = new();

        for (int i = 0; i < baseStock.Count; i++)
        {
            ShopStockEntry entry = baseStock[i];

            if (entry == null || entry.item == null)
                continue;

            displaySlots.Add(new InventorySlot(entry.item, 1));
        }

        return displaySlots;
    }
}