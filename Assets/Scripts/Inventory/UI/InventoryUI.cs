using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private TMP_Text inventoryTitleText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private CharacterEquipment characterEquipment;
    [SerializeField] private ItemTooltipUI itemTooltipUI;
    [SerializeField] private ItemContextMenuUI itemContextMenuUI;

    [Header("Mode")]
    [SerializeField] private InventoryPanelMode panelMode = InventoryPanelMode.Player;

    [Header("Input")]
    [SerializeField] private bool allowToggleWithKey = true;
    [SerializeField] private Key toggleKey = Key.Tab;

    private readonly List<InventorySlotUI> spawnedSlots = new();

    private IReadOnlyList<InventorySlot> currentSlots;
    private int currentMaxSlots;
    private bool showGold;
    private string currentTitle = "Inventory";

    private LootContainer boundLootContainer;
    private PlayerInventory boundPlayerInventory;
    private InventoryUI linkedPanel;

    private void Start()
    {
        inventoryPanel.SetActive(false);
        itemTooltipUI.Hide();
        itemContextMenuUI.Hide();

        if (playerInventory != null)
        {
            BindPlayerInventory(playerInventory, currentTitle);
        }
        else
        {
            currentSlots = System.Array.Empty<InventorySlot>();
            currentMaxSlots = 0;
        }
    }

    private void Update()
    {
        if (!allowToggleWithKey)
            return;

        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    public void SetLinkedPanel(InventoryUI other)
    {
        linkedPanel = other;
    }

    public void SetMode(InventoryPanelMode mode)
    {
        panelMode = mode;
    }

    public void Show()
    {
        inventoryPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        RefreshUI();
    }

    public void Hide()
    {
        inventoryPanel.SetActive(false);
        itemTooltipUI.Hide();
        itemContextMenuUI.Hide();
    }

    public void ToggleInventory()
    {
        bool newState = !inventoryPanel.activeSelf;

        if (newState)
        {
            if (currentSlots == null && playerInventory != null)
            {
                BindPlayerInventory(playerInventory, currentTitle);
            }

            Show();
        }
        else
        {
            Hide();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void BindPlayerInventory(PlayerInventory inventory, string title = "Inventory")
    {
        if (inventory == null)
        {
            Debug.LogWarning("BindPlayerInventory called with null inventory.", this);
            return;
        }

        boundLootContainer = null;
        boundPlayerInventory = inventory;

        currentTitle = title;
        showGold = true;

        BindInventory(inventory.Slots, inventory.MaxSlots);
    }

    public void BindLoot(LootContainer loot, PlayerInventory player, string title = "Loot")
    {
        if (loot == null)
        {
            Debug.LogWarning("BindLoot called with null loot container.", this);
            return;
        }

        boundLootContainer = loot;
        boundPlayerInventory = player;

        currentTitle = title;
        showGold = false;

        BindInventory(loot.StoredItems, loot.StoredItems.Count);
    }

    public void BindInventory(IReadOnlyList<InventorySlot> slots, int maxSlots)
    {
        currentSlots = slots ?? System.Array.Empty<InventorySlot>();
        currentMaxSlots = Mathf.Max(0, maxSlots);

        RebuildUI();
    }

    public void RebuildUI()
    {
        ClearSlots();

        for (int i = 0; i < currentMaxSlots; i++)
        {
            InventorySlotUI slotUI = Instantiate(slotPrefab, slotContainer);
            slotUI.Initialize(this, i);
            spawnedSlots.Add(slotUI);
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (inventoryTitleText != null)
        {
            inventoryTitleText.text = currentTitle;
        }

        if (goldText != null)
        {
            goldText.gameObject.SetActive(showGold);

            if (showGold && boundPlayerInventory != null)
            {
                goldText.text = $"Gold: {boundPlayerInventory.Gold}";
            }
        }

        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            InventorySlotUI slotUI = spawnedSlots[i];
            InventorySlot slot = GetSlotAt(i);

            if (slot == null || slot.IsEmpty)
            {
                slotUI.SetEmpty();
                continue;
            }

            ItemData item = slot.item;
            bool isEquipped = characterEquipment != null && ShouldShowEquippedHighlight() && characterEquipment.IsSlotEquipped(i);

            slotUI.SetItem(item.icon, slot.quantity, isEquipped);
        }
    }

    public void OnSlotHovered(int slotIndex)
    {
        InventorySlot slot = GetSlotAt(slotIndex);

        if (slot == null || slot.IsEmpty)
        {
            itemTooltipUI.Hide();
            return;
        }

        itemTooltipUI.Show(slot.item);
    }

    public void OnSlotHoverExited(int slotIndex)
    {
        itemTooltipUI.Hide();
    }

    public void OnSlotLeftClicked(int slotIndex)
    {
        InventorySlot slot = GetSlotAt(slotIndex);

        if (slot == null || slot.IsEmpty)
            return;

        switch (panelMode)
        {
            case InventoryPanelMode.Player:
            case InventoryPanelMode.PlayerWhileLooting:
                HandlePlayerLeftClick(slotIndex, slot);
                break;

            case InventoryPanelMode.LootSource:
                TakeLootItem(slotIndex, slot);
                break;

            case InventoryPanelMode.ShopStock:
            case InventoryPanelMode.PlayerWhileShopping:
                // Shop behavior later.
                break;
        }
    }

    public void OnSlotRightClicked(int slotIndex)
    {
        InventorySlot slot = GetSlotAt(slotIndex);

        if (slot == null || slot.IsEmpty)
            return;

        switch (panelMode)
        {
            case InventoryPanelMode.Player:
            case InventoryPanelMode.PlayerWhileLooting:
                ShowPlayerContextMenu(slotIndex, slot);
                break;

            case InventoryPanelMode.LootSource:
                ShowLootContextMenu(slotIndex, slot);
                break;

            case InventoryPanelMode.ShopStock:
            case InventoryPanelMode.PlayerWhileShopping:
                ShowShopContextMenu(slotIndex, slot);
                break;
        }
    }

    private void HandlePlayerLeftClick(int slotIndex, InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
            return;

        if (characterEquipment != null && IsEquippable(slot.item))
        {
            if (characterEquipment.ToggleEquip(slot.item, slotIndex))
            {
                RefreshUI();
                linkedPanel?.RefreshUI();
            }

            return;
        }

        Debug.Log($"Clicked item: {slot.item.itemName}");
    }

    private void ShowPlayerContextMenu(int slotIndex, InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
            return;

        ItemData item = slot.item;

        if (characterEquipment != null && IsEquippable(item))
        {
            bool isEquipped = characterEquipment.IsSlotEquipped(slotIndex);
            string label = isEquipped ? "Unequip" : "Equip";
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            itemContextMenuUI.Show(
                mousePosition,
                label,
                () =>
                {
                    characterEquipment.ToggleEquip(item, slotIndex);
                    RefreshUI();
                    linkedPanel?.RefreshUI();
                });

            return;
        }

        Debug.Log($"Right clicked: {item.itemName}");
    }

    private void ShowLootContextMenu(int slotIndex, InventorySlot slot)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        itemContextMenuUI.Show(
            mousePosition,
            "Take",
            () => TakeLootItem(slotIndex, slot));
    }

    private void ShowShopContextMenu(int slotIndex, InventorySlot slot)
    {
        Debug.Log("Shop context menu not implemented yet.");
    }

    private InventorySlot GetSlotAt(int slotIndex)
    {
        if (currentSlots == null)
            return null;

        if (slotIndex < 0 || slotIndex >= currentMaxSlots)
            return null;

        if (slotIndex >= currentSlots.Count)
            return null;

        return currentSlots[slotIndex];
    }

    private bool ShouldShowEquippedHighlight()
    {
        return panelMode == InventoryPanelMode.Player ||
               panelMode == InventoryPanelMode.PlayerWhileLooting ||
               panelMode == InventoryPanelMode.PlayerWhileShopping;
    }

    private bool IsEquippable(ItemData item)
    {
        return item is WeaponItemData || item is ArmorItemData;
    }

    private void ClearSlots()
    {
        foreach (InventorySlotUI slot in spawnedSlots)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }

        spawnedSlots.Clear();
    }

    private void TakeLootItem(int slotIndex, InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
            return;

        if (boundLootContainer == null || boundPlayerInventory == null)
        {
            Debug.LogWarning("Loot panel is missing loot/player bindings.", this);
            return;
        }

        bool taken = boundLootContainer.TakeItemAt(slotIndex, boundPlayerInventory);

        if (!taken)
            return;

        BindInventory(boundLootContainer.StoredItems, boundLootContainer.StoredItems.Count);
        linkedPanel?.RefreshUI();

        if (boundLootContainer == null)
            return;

        if (boundLootContainer.IsEmpty)
        {
            FindFirstObjectByType<InventoryWindowController>()?.CloseAll();
        }
    }
}