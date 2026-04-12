using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private string enemyName = "Goblin";
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int currentHealth = 50;

    public string EnemyName => enemyName;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{enemyName} health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{enemyName} died");
        Destroy(gameObject);
    }
}