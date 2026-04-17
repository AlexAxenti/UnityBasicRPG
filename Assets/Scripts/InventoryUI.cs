using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private CharacterEquipment characterEquipment;
    [SerializeField] private ItemTooltipUI itemTooltipUI;
    [SerializeField] private ItemContextMenuUI itemContextMenuUI;


    [Header("Input")]
    [SerializeField] private Key toggleKey = Key.Tab;

    private readonly List<InventorySlotUI> spawnedSlots = new();

    private void Start()
    {
        inventoryPanel.SetActive(false);
        itemTooltipUI.Hide();
        itemContextMenuUI.Hide();
        RebuildUI();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        bool newState = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(newState);

        if (newState)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            RefreshUI();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            itemTooltipUI.Hide();
            itemContextMenuUI.Hide();
        }
    }

    public void RebuildUI()
    {
        ClearSlots();

        for (int i = 0; i < playerInventory.MaxSlots; i++)
        {
            InventorySlotUI slotUI = Instantiate(slotPrefab, slotContainer);
            slotUI.Initialize(this, i);
            spawnedSlots.Add(slotUI);
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            InventorySlotUI slotUI = spawnedSlots[i];

            InventorySlot slot = playerInventory.GetItemAtSlot(i);

            if (slot == null || slot.IsEmpty)
            {
                slotUI.SetEmpty();
                continue;
            }

            ItemData item = slot.item;
            bool isEquipped = IsItemEquipped(item);

            slotUI.SetItem(item.icon, slot.quantity, isEquipped);
        }
    }

    public void OnSlotHovered(int slotIndex)
    {
        InventorySlot slot = playerInventory.GetItemAtSlot(slotIndex);

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
        Debug.Log($"Left clicked: {slotIndex}");
        InventorySlot slot = playerInventory.GetItemAtSlot(slotIndex);

        if (slot == null || slot.IsEmpty)
            return;

        if (slot.item is WeaponItemData weapon)
        {
            if (characterEquipment.EquippedWeapon == weapon)
            {
                characterEquipment.UnequipWeapon();
                Debug.Log($"Unequipped weapon: {weapon.itemName}");
            }
            else
            {
                characterEquipment.EquipWeapon(weapon);
                Debug.Log($"Equipped weapon: {weapon.itemName}");
            }

            RefreshUI();
            return;
        }

        Debug.Log($"Clicked item: {slot.item.itemName}");
    }

    public void OnSlotRightClicked(int slotIndex)
    {
        Debug.Log($"Right clicked: {slotIndex}");
        InventorySlot slot = playerInventory.GetItemAtSlot(slotIndex);

        if (slot == null || slot.IsEmpty)
            return;

        ItemData item = slot.item;

        if (item is WeaponItemData weapon)
        {
            bool isEquipped = characterEquipment.EquippedWeapon == weapon;
            string label = isEquipped ? "Unequip" : "Equip";

            Vector2 mousePosition = Mouse.current.position.ReadValue();

            itemContextMenuUI.Show(
                mousePosition,
                label,
                () =>
                {
                    if (isEquipped)
                    {
                        characterEquipment.UnequipWeapon();
                        Debug.Log($"Unequipped weapon: {weapon.itemName}");
                    }
                    else
                    {
                        characterEquipment.EquipWeapon(weapon);
                        Debug.Log($"Equipped weapon: {weapon.itemName}");
                    }

                    RefreshUI();
                });
            return;
        }

        Debug.Log($"Right clicked: {item.itemName}");
    }

    private bool IsItemEquipped(ItemData item)
    {
        if (item == null || characterEquipment == null)
            return false;

        return characterEquipment.IsItemEquipped(item);
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
}