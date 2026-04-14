using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float armor = 5;
    [SerializeField] private float movementSpeed = 5;

    public float MaxHealth => maxHealth;
    public float Damage => damage;
    public float AttackRange => attackRange;
    public float Armor => armor;
    public float MovementSpeed => movementSpeed;
}
