using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "RPG/Items/Weapon")]
public class WeaponItemData : ItemData
{
    //TODO remove public
    [Header("Combat")]
    public int damageBonus = 0;
    public float attackRange = 0f;

    [Header("Visual")]
    public GameObject equippedPrefab;

    private void OnValidate()
    {
        category = ItemCategory.Weapon;
        maxStack = 1;
    }
}