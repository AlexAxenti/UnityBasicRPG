using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] private List<LootTableEntry> entries = new();

    public List<InventorySlot> RollLoot()
    {
        List<InventorySlot> droppedItems = new();

        foreach (LootTableEntry entry in entries)
        {
            if (entry.item == null) continue;
            if (entry.maxQuantity < entry.minQuantity) continue;

            float roll = Random.value;
            if (roll > entry.dropChance) continue;

            int quantity = Random.Range(entry.minQuantity, entry.maxQuantity + 1);
            if (quantity <= 0) continue;

            droppedItems.Add(new InventorySlot(entry.item, quantity));
        }

        return droppedItems;
    }
}