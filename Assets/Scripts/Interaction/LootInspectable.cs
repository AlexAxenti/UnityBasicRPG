using UnityEngine;

public class LootInspectable : Inspectable
{
    [SerializeField] private LootContainer lootPouch;
    private InventoryWindowController inventoryWindowController;

    private void Awake()
    {
        inventoryWindowController = FindAnyObjectByType<InventoryWindowController>();  
    }

  private void Reset()
    {
        lootPouch = GetComponent<LootContainer>();
    }

    public override bool CanInteract()
    {
        return lootPouch != null;
    }

    public override void Interact(PlayerInteractor interactor)
    {
        if (lootPouch == null) return;

        PlayerInventory playerInventory = interactor.GetComponent<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory not found on PlayerInteractor.");
            return;
        }

        // lootPouch.LootAll(playerInventory);
        
        if (inventoryWindowController == null)
            return;

        if (inventoryWindowController.IsLootOpen && inventoryWindowController.ActiveLootContainer == lootPouch)
        {
            inventoryWindowController.CloseAll();
            return;
        }

        inventoryWindowController.OpenLoot(lootPouch);
    }
}