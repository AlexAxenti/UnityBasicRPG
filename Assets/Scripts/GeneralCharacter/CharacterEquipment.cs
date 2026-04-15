using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [Header("Equipped Items")]
    [SerializeField] private WeaponItemData equippedWeapon;

    [Header("Sockets")]
    [SerializeField] private Transform weaponSocket;

    private GameObject equippedWeaponInstance;

    public WeaponItemData EquippedWeapon => equippedWeapon;

    public float DamageBonus => equippedWeapon != null ? equippedWeapon.DamageBonus : 0f;
    public float AttackRange => equippedWeapon != null ? equippedWeapon.AttackRange : 0f;

    private void Start()
    {
        RefreshWeaponVisual();
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
}