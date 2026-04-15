using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "RPG/Equipment/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "New Weapon";
    public float damageBonus = 0;
    public float attackRange = 2f;
}
