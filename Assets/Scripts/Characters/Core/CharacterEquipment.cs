using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [Header("Equipped Items")]
    [SerializeField] private WeaponItemData equippedWeapon;
    [SerializeField] private ArmorItemData equippedArmor;

    [Header("Sockets")]
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private Transform armorSocket;

    private GameObject equippedWeaponInstance;
    private GameObject equippedArmorInstance;

    private int equippedWeaponSlotIndex = -1;
    private int equippedArmorSlotIndex = -1;

    public WeaponItemData EquippedWeapon => equippedWeapon;
    public ArmorItemData EquippedArmor => equippedArmor;

    public float DamageBonus => equippedWeapon != null ? equippedWeapon.DamageBonus : 0f;
    public float AttackRange => equippedWeapon != null ? equippedWeapon.AttackRange : 0f;
    public float ArmorBonus => equippedArmor != null ? equippedArmor.ArmorBonus : 0f;

    private void Start()
    {
        RefreshWeaponVisual();
        RefreshArmorVisual();
    }

    public bool ToggleEquip(ItemData item, int slotIndex)
    {
        if (item == null)
            return false;

        if (item is WeaponItemData weapon)
        {
            if (equippedWeapon == weapon && equippedWeaponSlotIndex == slotIndex)
            {
                UnequipWeaponInternal();
            }
            else
            {
                EquipWeaponInternal(weapon, slotIndex);
            }

            return true;
        }

        if (item is ArmorItemData armor)
        {
            if (equippedArmor == armor && equippedArmorSlotIndex == slotIndex)
            {
                UnequipArmorInternal();
            }
            else
            {
                EquipArmorInternal(armor, slotIndex);
            }

            return true;
        }

        return false;
    }

    public bool IsItemEquipped(ItemData item)
    {
        if (item == null)
            return false;

        if (item is WeaponItemData weapon)
            return equippedWeapon == weapon;

        if (item is ArmorItemData armor)
            return equippedArmor == armor;

        return false;
    }

    public bool IsSlotEquipped(int slotIndex)
    {
        return slotIndex >= 0 &&
               (slotIndex == equippedWeaponSlotIndex || slotIndex == equippedArmorSlotIndex);
    }

    private void EquipWeaponInternal(WeaponItemData newWeapon, int slotIndex)
    {
        equippedWeapon = newWeapon;
        equippedWeaponSlotIndex = slotIndex;
        RefreshWeaponVisual();
    }

    private void UnequipWeaponInternal()
    {
        equippedWeapon = null;
        equippedWeaponSlotIndex = -1;
        RefreshWeaponVisual();
    }

    private void EquipArmorInternal(ArmorItemData newArmor, int slotIndex)
    {
        equippedArmor = newArmor;
        equippedArmorSlotIndex = slotIndex;
        RefreshArmorVisual();
    }

    private void UnequipArmorInternal()
    {
        equippedArmor = null;
        equippedArmorSlotIndex = -1;
        RefreshArmorVisual();
    }

    private void RefreshWeaponVisual()
    {
        if (equippedWeaponInstance != null)
        {
            Destroy(equippedWeaponInstance);
            equippedWeaponInstance = null;
        }

        if (equippedWeapon == null)
            return;

        if (weaponSocket == null)
        {
            Debug.LogWarning($"{name} has no weapon socket assigned.", this);
            return;
        }

        if (equippedWeapon.EquippedPrefab == null)
        {
            Debug.LogWarning($"{name}'s equipped weapon has no prefab assigned.", this);
            return;
        }

        equippedWeaponInstance = Instantiate(equippedWeapon.EquippedPrefab, weaponSocket);
        equippedWeaponInstance.transform.localPosition = Vector3.zero;
        equippedWeaponInstance.transform.localRotation = Quaternion.identity;
        equippedWeaponInstance.transform.localScale = Vector3.one;
    }

    private void RefreshArmorVisual()
    {
        if (equippedArmorInstance != null)
        {
            Destroy(equippedArmorInstance);
            equippedArmorInstance = null;
        }

        if (equippedArmor == null)
            return;

        if (armorSocket == null)
        {
            Debug.LogWarning($"{name} has no armor socket assigned.", this);
            return;
        }

        if (equippedArmor.EquippedPrefab == null)
        {
            Debug.LogWarning($"{name}'s equipped armor has no prefab assigned.", this);
            return;
        }

        equippedArmorInstance = Instantiate(equippedArmor.EquippedPrefab, armorSocket);
        equippedArmorInstance.transform.localPosition = Vector3.zero;
        equippedArmorInstance.transform.localRotation = Quaternion.identity;
        equippedArmorInstance.transform.localScale = Vector3.one;
    }
}