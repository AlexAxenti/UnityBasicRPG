using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private int level = 1;

    [Header("Base Stats")]
    [SerializeField] private float baseMaxHealth = 100;
    [SerializeField] private float baseDamage = 10;
    [SerializeField] private float baseAttackRange = 1.5f;
    [SerializeField] private float baseArmor = 5;
    [SerializeField] private float baseMovementSpeed = 5;

    [Header("Growth Per Level")]
    [SerializeField] private float maxHealthPerLevel = 10f;
    [SerializeField] private float damagePerLevel = 2f;
    [SerializeField] private float armorPerLevel = 1f;

    public int Level => level;

    public float MaxHealth => baseMaxHealth + maxHealthPerLevel * (level - 1);
    public float Damage => baseDamage + damagePerLevel * (level - 1);
    public float AttackRange => baseAttackRange;
    public float Armor => baseArmor + armorPerLevel * (level - 1);
    public float MovementSpeed => baseMovementSpeed;

    public void SetLevel(int newLevel)
    {
        level = Mathf.Max(1, newLevel);
    }

    public void IncreaseLevel(int amount = 1)
    {
        level = Mathf.Max(1, level + amount);
    }
}
