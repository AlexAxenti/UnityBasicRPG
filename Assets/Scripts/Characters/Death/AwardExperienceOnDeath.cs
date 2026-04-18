using UnityEngine;

public class AwardExperienceOnDeath : MonoBehaviour
{
    [SerializeField] private int experienceAmount = 10;

    private CharacterHealth characterHealth;

    private void Awake()
    {
        characterHealth = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        if (characterHealth != null)
            characterHealth.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        if (characterHealth != null)
            characterHealth.OnDied -= HandleDied;
    }

    private void HandleDied(CharacterHealth health)
    {
        PlayerProgression playerProgression = FindAnyObjectByType<PlayerProgression>();
        if (playerProgression == null) return;

        playerProgression.GainExperience(experienceAmount);
    }
}