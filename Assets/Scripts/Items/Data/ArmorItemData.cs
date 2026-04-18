using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItemData", menuName = "RPG/Items/Armor")]
public class ArmorItemData : ItemData
{
    [Header("Combat")]
    [SerializeField] private float armorBonus = 1f;

    [Header("Visual")]
    [SerializeField] private GameObject equippedPrefab;
    [SerializeField] private ArmorType armorType;

    public float ArmorBonus => armorBonus;
    public GameObject EquippedPrefab => equippedPrefab;
    public ArmorType ArmorType => armorType;

    private void OnValidate()
    {
        category = ItemCategory.Armor;
        maxStack = 1;
    }
}

public enum ArmorType
{
    Helmet,
    Chest,
    Legs,
    Gloves,
    Boots,
}