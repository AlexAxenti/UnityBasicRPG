using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipmentTester : MonoBehaviour
{
    private PlayerInventory inventory;
    private CharacterEquipment characterEquipment;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        characterEquipment = GetComponent<CharacterEquipment>();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ToggleWeapon();
        }
    }

    private void ToggleWeapon()
    {
        if (inventory == null || characterEquipment == null)
            return;

        if (characterEquipment.EquippedWeapon != null)
        {
            characterEquipment.UnequipWeapon();
            Debug.Log("Weapon unequipped.");
            return;
        }

        WeaponItemData firstWeapon = inventory.FindFirstWeapon();

        if (firstWeapon == null)
        {
            Debug.Log("No weapon found in inventory.");
            return;
        }

        characterEquipment.EquipWeapon(firstWeapon);
        Debug.Log($"Equipped weapon: {firstWeapon.itemName}");
    }
}