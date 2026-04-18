using System.Collections.Generic;
using UnityEngine;

public class LootContainer : MonoBehaviour
{
    [Header("Optional Fixed Contents")]
    [SerializeField] private bool useStartingItems = false;
    [SerializeField] private List<InventorySlot> startingItems = new();

    [Header("Runtime Contents")]
    [SerializeField] private List<InventorySlot> storedItems = new();

    //TODO remove below? not needed
    public bool IsEmpty => storedItems == null || storedItems.Count == 0;
    public IReadOnlyList<InventorySlot> StoredItems => storedItems;

    private void Awake()
    {
        if (useStartingItems)
        {
            Initialize(startingItems);
        }
    }

    public void Initialize(List<InventorySlot> items)
    {
        storedItems = new List<InventorySlot>();

        if (items == null) return;

        foreach (InventorySlot slot in items)
        {
            if (slot == null || slot.IsEmpty) continue;

            storedItems.Add(new InventorySlot(slot.item, slot.quantity));
        }
    }

    public void LootAll(PlayerInventory playerInventory)
    {
        if (playerInventory == null) return;

        if (storedItems == null || storedItems.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        List<InventorySlot> remainingItems = new();

        foreach (InventorySlot slot in storedItems)
        {
            if (slot == null || slot.IsEmpty)
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