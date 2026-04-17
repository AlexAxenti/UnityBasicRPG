using System.Collections.Generic;
using UnityEngine;

public class LootPouch : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> storedItems = new();

    public void Initialize(List<InventorySlot> items)
    {
        storedItems = items;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory == null) return;

        AutoLoot(playerInventory);
    }

    private void AutoLoot(PlayerInventory playerInventory)
    {
        if (storedItems == null || storedItems.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        List<InventorySlot> remainingItems = new();

        foreach (InventorySlot slot in storedItems)
        {
            if (slot.IsEmpty)
                continue;

            bool added = playerInventory.AddItem(slot.item, slot.quantity);

            if (!added)
            {
                remainingItems.Add(slot);
            }
            else
            {
                Debug.Log($"Picked up {slot.quantity}x {slot.item.itemName}");
            }
        }

        storedItems = remainingItems;

        if (storedItems.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}