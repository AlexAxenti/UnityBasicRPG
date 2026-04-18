using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "RPG/Items/Weapon")]
public class WeaponItemData : ItemData
{
    [Header("Combat")]
    [SerializeField] private float damageBonus = 0;
    [SerializeField] private float attackRange = 0f;

    [Header("Visual")]
    [SerializeField] private GameObject equippedPrefab;

    public float DamageBonus => damageBonus;
    public float AttackRange => attackRange;
    public GameObject EquippedPrefab => equippedPrefab;

    private void OnValidate()
    {
        category = ItemCategory.Weapon;
        maxStack = 1;
    }
}