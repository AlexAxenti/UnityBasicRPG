using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [Header("Equipped Items")]
    [SerializeField] private WeaponData equippedWeapon;

    public WeaponData EquippedWeapon => equippedWeapon;

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

    public void EquipWeapon(WeaponData newWeapon)
    {
        equippedWeapon = newWeapon;
    }
}