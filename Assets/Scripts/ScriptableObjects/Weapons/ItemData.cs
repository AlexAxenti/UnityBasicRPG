using UnityEngine;

public enum ItemCategory
{
    Weapon,
    Armor,
    Consumable,
    Material,
    Quest,
    Gold,
    Misc
}

public abstract class ItemData : ScriptableObject
{
    //TODO remove public
    [Header("Base Info")]
    public string itemId;
    public string itemName;
    [TextArea] public string description;

    [Header("Display")]
    public Sprite icon;

    [Header("Economy")]
    public int value = 1;
    public float weight = 0f;

    [Header("Inventory")]
    public int maxStack = 1;
    public ItemCategory category;
}