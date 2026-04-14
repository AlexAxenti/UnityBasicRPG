using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    private CharacterInfo characterInfo;
    private CharacterStats characterStats;
    private float currentHealth;

    //TODO move to seperate regen component later
    [SerializeField] private bool canRegenerateHealth = false;
    [SerializeField] private float healthRegenAmount = 5f;
    [SerializeField] private float healthRegenInterval = 2f;
    private float regenTimer = 0f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => characterStats != null ? characterStats.MaxHealth : 0f;

    //TODO move later
    private ExperienceReward experienceReward;

    private void Awake()
    {
        characterInfo = GetComponent<CharacterInfo>();
        characterStats = GetComponent<CharacterStats>();
        if (characterStats != null)
        {
            currentHealth = characterStats.MaxHealth;
        }
        else
        {
            Debug.LogError("CharacterStats component not found on " + gameObject.name);
            currentHealth = 100;
        }

        experienceReward = GetComponent<ExperienceReward>();
    }

    private void Update()
    {
        HandleRegen();
    }

    private void HandleRegen()
    {

        if (!canRegenerateHealth)
            return;

        if (currentHealth >= characterStats.MaxHealth || currentHealth <= 0)
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
        currentHealth = Mathf.Min(currentHealth, characterStats.MaxHealth);

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
        AwardExperience();

        Debug.Log($"{characterInfo.CharacterName} died");
        Destroy(gameObject);
    }

    private void AwardExperience()
    {
        if (experienceReward == null) return;

        PlayerProgression playerProgression = FindAnyObjectByType<PlayerProgression>();
        if (playerProgression == null) return;

        playerProgression.GainExperience(experienceReward.ExperienceAmount);
    }
}