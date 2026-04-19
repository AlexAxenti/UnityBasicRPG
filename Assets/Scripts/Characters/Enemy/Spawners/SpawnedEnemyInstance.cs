using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public class SpawnedEnemyInstance : MonoBehaviour
{
    private EnemySpawner ownerSpawner;
    private CharacterHealth characterHealth;
    private bool hasReportedDeath;

    public void Initialize(EnemySpawner spawner)
    {
        ownerSpawner = spawner;
        characterHealth = GetComponent<CharacterHealth>();

        if (characterHealth == null)
        {
            Debug.LogWarning($"SpawnedEnemyInstance on {name} could not find CharacterHealth.", this);
            return;
        }

        characterHealth.OnDied += HandleDied;
    }

    private void HandleDied(CharacterHealth health)
    {
        if (hasReportedDeath)
        {
            return;
        }

        hasReportedDeath = true;

        if (ownerSpawner != null)
        {
            ownerSpawner.NotifyEnemyDied(this);
        }
    }

    private void OnDestroy()
    {
        if (characterHealth != null)
        {
            characterHealth.OnDied -= HandleDied;
        }
    }
}