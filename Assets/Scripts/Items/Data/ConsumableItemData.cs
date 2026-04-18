using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "RPG/Items/Consumable")]
public class ConsumableItemData : ItemData
{
    //TODO remove public
    public int healthRestore = 0;
    public int manaRestore = 0;

    private void OnValidate()
    {
        category = ItemCategory.Consumable;
        if (maxStack < 1) maxStack = 1;
    }
}