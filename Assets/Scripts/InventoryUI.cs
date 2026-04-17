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

    [Header("Input")]
    [SerializeField] private Key toggleKey = Key.Tab;

    private readonly List<InventorySlotUI> spawnedSlots = new();

    private void Start()
    {
        inventoryPanel.SetActive(false);
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

    public void OnSlotClicked(int slotIndex)
    {
        Debug.Log($"InventoryUI received click for slot {slotIndex}");

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

        Debug.Log($"Clicked item: {slot.item.itemName} (no click behavior yet)");
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