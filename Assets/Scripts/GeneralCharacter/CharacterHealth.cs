using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    private CharacterInfo characterInfo;
    private CharacterStats characterStats;
    private float maxHealth;
    private float currentHealth;

    [SerializeField] private bool canRegenerateHealth = false;
    [SerializeField] private float healthRegenAmount = 5f;
    [SerializeField] private float healthRegenInterval = 2f;
    private float regenTimer = 0f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        characterInfo = GetComponent<CharacterInfo>();
        characterStats = GetComponent<CharacterStats>();
        if (characterStats != null)
        {
            maxHealth = characterStats.MaxHealth;
            currentHealth = maxHealth;
        }
        else
        {
            Debug.LogError("CharacterStats component not found on " + gameObject.name);
            maxHealth = 100;
            currentHealth = maxHealth;
        }
    }

    private void Update()
    {
        HandleRegen();
    }

    private void HandleRegen()
    {

        if (!canRegenerateHealth)
            return;

        if (currentHealth >= maxHealth || currentHealth <= 0)
        {
            regenTimer = 0f;
            return;
        }

        regenTimer += Time.deltaTime;

        if (regenTimer >= healthRegenInterval)
        {
            Heal(healthRegenAmount);
            regenTimer = 0f;
        }

    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"{characterInfo.CharacterName} healed to {currentHealth}");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{characterInfo.CharacterName} health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{characterInfo.CharacterName} died");
        Destroy(gameObject);
    }
}