using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    //TODO create some sort of multiplier by level
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int experienceToNextLevel = 100;

    private CharacterStats characterStats;
    private CharacterHealth characterHealth;

    public int Level => characterStats != null ? characterStats.Level : 1;
    public int CurrentExperience => currentExperience;
    public int ExperienceToNextLevel => experienceToNextLevel;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterHealth = GetComponent<CharacterHealth>();

        if (characterStats == null)
        {
            Debug.LogError("CharacterStats component not found on " + gameObject.name);
        }

        if (characterHealth == null)
        {
            Debug.LogError("CharacterHealth component not found on " + gameObject.name);
        }
    }

    public void GainExperience(int amount)
    {
        currentExperience += amount;
        Debug.Log($"Gained {amount} XP. Current XP: {currentExperience}/{experienceToNextLevel}");

        while (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (characterStats != null)
        {
            characterStats.IncreaseLevel();
        }

        //TODO set exp multiplier in a better way later
        experienceToNextLevel += 50;

        Debug.Log($"Level up! Reached level {characterStats.Level}. XP for next level: {experienceToNextLevel}");
    }
}