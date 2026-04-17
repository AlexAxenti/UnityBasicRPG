using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 20;
    [SerializeField] private List<InventorySlot> slots = new();
    [SerializeField] private int gold = 0;

    public IReadOnlyList<InventorySlot> Slots => slots;
    public int Gold => gold;
    public int MaxSlots => maxSlots;

    //TODO remove after testing
    [Header("Debug / Starting Items")]
    [SerializeField] private WeaponItemData startingWeapon;

    private void Start()
    {
        if (startingWeapon != null && !HasItem(startingWeapon))
        {
            AddItem(startingWeapon, 1);
        }
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        if (item.maxStack > 1)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == item && slots[i].quantity < item.maxStack)
                {
                    int space = item.maxStack - slots[i].quantity;
                    int toAdd = Mathf.Min(space, amount);
                    slots[i].quantity += toAdd;
                    amount -= toAdd;

                    if (amount <= 0)
                        return true;
                }
            }
        }

        while (amount > 0)
        {
            if (slots.Count >= maxSlots)
                return false;

            int stackAmount = Mathf.Min(item.maxStack, amount);
            slots.Add(new InventorySlot(item, stackAmount));
            amount -= stackAmount;
        }

        return true;
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].item != item) continue;

            int toRemove = Mathf.Min(slots[i].quantity, amount);
            slots[i].quantity -= toRemove;
            amount -= toRemove;

            if (slots[i].quantity <= 0)
                slots.RemoveAt(i);

            if (amount <= 0)
                return true;
        }

        return false;
    }

    public InventorySlot GetItemAtSlot(int index)
    {
        if (index < 0 || index >= maxSlots || index >= slots.Count)
            return null;

        Debug.Log($"Looking for item at slot {index}: {(slots[index].IsEmpty ? "Empty" : slots[index].item.itemName)}");
        return slots[index];
    }

    //TODO potentially remove
    public bool HasItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        int total = 0;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                total += slots[i].quantity;
                if (total >= amount)
                    return true;
            }
        }

        return false;
    }

    //TODO remove after testing
    public WeaponItemData FindFirstWeapon()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item is WeaponItemData weapon && slots[i].quantity > 0)
                return weapon;
        }

        return null;
    }

    public void AddGold(int amount)
    {
        gold += Mathf.Max(0, amount);
    }
}