using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    //TODO instead of 1 armor handle 1 for each armor type enum
    [Header("Equipped Items")]
    [SerializeField] private WeaponItemData equippedWeapon;
    [SerializeField] private ArmorItemData equippedArmor;

    [Header("Sockets")]
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private Transform armorSocket;

    // Prefabs
    private GameObject equippedWeaponInstance;
    private GameObject equippedArmorInstance;

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

    public void EquipWeapon(WeaponItemData newWeapon)
    {
        equippedWeapon = newWeapon;
        RefreshWeaponVisual();
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
        RefreshWeaponVisual();
    }

    //TODO make more generic, just 1 method for equip is all thats needed
    public void EquipArmor(ArmorItemData newArmor)
    {
        equippedArmor = newArmor;
        RefreshArmorVisual();
    }

    public void UnequipArmor()
    {
        equippedArmor = null;
        RefreshArmorVisual();
    }

    public bool IsItemEquipped(ItemData item)
    {
        if (item == null)
            return false;

        return equippedWeapon == item || equippedArmor == item;
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

        equippedWeaponInstance = Instantiate(
            equippedWeapon.EquippedPrefab,
            weaponSocket
        );

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

        equippedArmorInstance = Instantiate(
            equippedArmor.EquippedPrefab,
            armorSocket
        );

        equippedArmorInstance.transform.localPosition = Vector3.zero;
        equippedArmorInstance.transform.localRotation = Quaternion.identity;
        equippedArmorInstance.transform.localScale = Vector3.one;
    }
}