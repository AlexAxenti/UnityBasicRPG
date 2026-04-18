using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    private CharacterInfo characterInfo;
    private CharacterStats characterStats;
    private CharacterEquipment characterEquipment;
    private float currentHealth;

    //TODO move to seperate regen component later
    [SerializeField] private bool canRegenerateHealth = false;
    [SerializeField] private float healthRegenAmount = 5f;
    [SerializeField] private float healthRegenInterval = 2f;
    private float regenTimer = 0f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => characterStats != null ? characterStats.MaxHealth : 0f;

    public event Action<CharacterHealth> OnDied;
    private bool isDead;
    public bool IsDead => isDead;
    //TODO move later
    // private ExperienceReward experienceReward;

    private void Awake()
    {
        characterInfo = GetComponent<CharacterInfo>();
        characterStats = GetComponent<CharacterStats>();
        characterEquipment = GetComponent<CharacterEquipment>();
        if (characterStats != null)
        {
            currentHealth = characterStats.MaxHealth;
        }
        else
        {
            Debug.LogError("CharacterStats component not found on " + gameObject.name);
            currentHealth = 100;
        }

        if (characterInfo == null)
        {
            Debug.LogError("CharacterInfo component not found on " + gameObject.name);
        }

        if (characterEquipment == null)
        {
            Debug.LogError("CharacterEquipment component not found on " + gameObject.name);
        }

        // experienceReward = GetComponent<ExperienceReward>();
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
        float armorBonus = characterEquipment != null ? characterEquipment.ArmorBonus : 0f;
        //TODO add better armor calculation later
        currentHealth -= Mathf.Max(damage - armorBonus, 0);
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{characterInfo.CharacterName} health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {      
        if (isDead) return;
        isDead = true;

        OnDied?.Invoke(this);
    }
}