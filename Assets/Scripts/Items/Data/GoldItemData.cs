using UnityEngine;

[CreateAssetMenu(fileName = "GoldItemData", menuName = "RPG/Items/Gold")]
public class GoldItemData : ItemData
{
    private void OnValidate()
    {
        category = ItemCategory.Gold;
        maxStack = int.MaxValue;
        value = 1;
    }
}