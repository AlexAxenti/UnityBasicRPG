using UnityEngine;

public class ShopService : MonoBehaviour
{
    [SerializeField] private ShopInventory shopInventory;

    private void Awake()
    {
        if (shopInventory == null)
        {
            shopInventory = GetComponent<ShopInventory>();
        }
    }

    public ShopInventory ShopInventory => shopInventory;

    public bool TryBuyBaseStockItem(int stockIndex, PlayerInventory playerInventory)
    {
        if (shopInventory == null || playerInventory == null)
            return false;

        if (stockIndex < 0 || stockIndex >= shopInventory.BaseStock.Count)
            return false;

        ShopStockEntry entry = shopInventory.BaseStock[stockIndex];

        if (entry == null || entry.item == null)
            return false;

        int price = entry.GetBuyPrice();

        if (playerInventory.Gold < price)
        {
            Debug.Log("Not enough gold.");
            return false;
        }

        bool added = playerInventory.AddItem(entry.item, 1);

        if (!added)
        {
            Debug.Log("Player inventory full.");
            return false;
        }

        playerInventory.RemoveGold(price);
        Debug.Log($"Bought {entry.item.itemName} for {price} gold.");
        return true;
    }

    public bool TryBuyResaleItem(int slotIndex, PlayerInventory playerInventory)
    {
        if (shopInventory == null || playerInventory == null)
            return false;

        if (slotIndex < 0 || slotIndex >= shopInventory.ResaleStock.Count)
            return false;

        InventorySlot slot = shopInventory.ResaleStock[slotIndex];

        if (slot == null || slot.IsEmpty)
            return false;

        int price = slot.item.value;

        if (playerInventory.Gold < price)
        {
            Debug.Log("Not enough gold.");
            return false;
        }

        bool added = playerInventory.AddItem(slot.item, 1);

        if (!added)
        {
            Debug.Log("Player inventory full.");
            return false;
        }

        playerInventory.RemoveGold(price);

        slot.quantity -= 1;
        if (slot.quantity <= 0)
        {
            shopInventory.RemoveResaleItemAt(slotIndex);
        }

        Debug.Log($"Bought resale item {slot.item.itemName} for {price} gold.");
        return true;
    }

    public bool TrySellItem(ItemData item, int quantity, PlayerInventory playerInventory)
    {
        if (item == null || quantity <= 0 || playerInventory == null || shopInventory == null)
            return false;

        int sellPrice = Mathf.Max(1, item.value / 2);

        bool removed = playerInventory.RemoveItem(item, quantity);
        if (!removed)
            return false;

        playerInventory.AddGold(sellPrice * quantity);
        shopInventory.AddResaleItem(item, quantity);

        Debug.Log($"Sold {quantity}x {item.itemName} for {sellPrice * quantity} gold.");
        return true;
    }
}