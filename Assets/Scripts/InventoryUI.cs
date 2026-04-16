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
            RefreshUI();
        }
    }

    public void RebuildUI()
    {
        ClearSlots();

        for (int i = 0; i < playerInventory.MaxSlots; i++)
        {
            InventorySlotUI slotUI = Instantiate(slotPrefab, slotContainer);
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