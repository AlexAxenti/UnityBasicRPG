using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxAlive = 5;
    [SerializeField] private float respawnDelay = 30f;

    [Header("Spawn Area")]
    [SerializeField] private bool useSpawnPoints = false;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnRadius = 10f;

    [Header("Debug")]
    [SerializeField] private bool spawnOnStart = true;

    private readonly List<SpawnedEnemyInstance> aliveEnemies = new();

    private void Start()
    {
        if (!spawnOnStart)
        {
            return;
        }

        SpawnInitialEnemies();
    }

    private void SpawnInitialEnemies()
    {
        int spawnCount = Mathf.Max(0, maxAlive);

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"EnemySpawner on {name} has no enemyPrefab assigned.", this);
            return;
        }

        if (aliveEnemies.Count >= maxAlive)
        {
            return;
        }

        Vector3 spawnPosition = GetSpawnPosition();
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        SpawnedEnemyInstance spawnedInstance = enemyObject.GetComponent<SpawnedEnemyInstance>();
        if (spawnedInstance == null)
        {
            spawnedInstance = enemyObject.AddComponent<SpawnedEnemyInstance>();
        }

        spawnedInstance.Initialize(this);
        aliveEnemies.Add(spawnedInstance);
    }

    private Vector3 GetSpawnPosition()
    {
        if (useSpawnPoints && spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform chosenPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            return chosenPoint.position;
        }

        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 localOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
        return transform.position + localOffset;
    }

    public void NotifyEnemyDied(SpawnedEnemyInstance deadEnemy)
    {
        if (deadEnemy == null)
        {
            return;
        }

        aliveEnemies.Remove(deadEnemy);
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (aliveEnemies.Count < maxAlive)
        {
            SpawnEnemy();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (!useSpawnPoints)
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}