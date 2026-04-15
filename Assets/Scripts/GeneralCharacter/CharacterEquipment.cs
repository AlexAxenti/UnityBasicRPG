using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [Header("Equipped Items")]
    [SerializeField] private WeaponItemData equippedWeapon;

    public WeaponItemData EquippedWeapon => equippedWeapon;

    public float GetWeaponDamageBonus()
    {
        if (equippedWeapon == null)
        {
            return 0;
        }

        return equippedWeapon.damageBonus;
    }

    public float GetAttackRange()
    {
        if (equippedWeapon == null)
        {
            return 2f;
        }

        return equippedWeapon.attackRange;
    }

    public void EquipWeapon(WeaponItemData newWeapon)
    {
        equippedWeapon = newWeapon;
    }
}