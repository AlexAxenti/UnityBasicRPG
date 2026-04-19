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
    private readonly Dictionary<SpawnedEnemyInstance, int> aliveSpawnPointIndices = new();

    private void Start()
    {
        if (!spawnOnStart)
            return;

        SpawnInitialEnemies();
    }

    private void SpawnInitialEnemies()
    {
        int spawnCount = GetInitialSpawnCount();

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    private int GetInitialSpawnCount()
    {
        int desiredCount = Mathf.Max(0, maxAlive);

        if (useSpawnPoints && spawnPoints != null && spawnPoints.Length > 0)
        {
            return Mathf.Min(desiredCount, spawnPoints.Length);
        }

        return desiredCount;
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"EnemySpawner on {name} has no enemyPrefab assigned.", this);
            return;
        }

        if (aliveEnemies.Count >= GetMaxAllowedAliveCount())
            return;

        Vector3 spawnPosition;
        int spawnPointIndex = -1;

        if (useSpawnPoints && spawnPoints != null && spawnPoints.Length > 0)
        {
            if (!TryGetFreeSpawnPoint(out spawnPointIndex, out Transform chosenPoint))
            {
                return;
            }

            spawnPosition = chosenPoint.position;
        }
        else
        {
            spawnPosition = GetRandomSpawnPosition();
        }

        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        SpawnedEnemyInstance spawnedInstance = enemyObject.GetComponent<SpawnedEnemyInstance>();
        if (spawnedInstance == null)
        {
            spawnedInstance = enemyObject.AddComponent<SpawnedEnemyInstance>();
        }

        spawnedInstance.Initialize(this);
        aliveEnemies.Add(spawnedInstance);

        if (spawnPointIndex >= 0)
        {
            aliveSpawnPointIndices[spawnedInstance] = spawnPointIndex;
        }
    }

    private int GetMaxAllowedAliveCount()
    {
        if (useSpawnPoints && spawnPoints != null && spawnPoints.Length > 0)
        {
            return Mathf.Min(maxAlive, spawnPoints.Length);
        }

        return Mathf.Max(0, maxAlive);
    }

    private bool TryGetFreeSpawnPoint(out int freeIndex, out Transform freePoint)
    {
        freeIndex = -1;
        freePoint = null;

        List<int> freeIndices = GetFreeSpawnPointIndices();

        if (freeIndices.Count == 0)
            return false;

        freeIndex = freeIndices[Random.Range(0, freeIndices.Count)];
        freePoint = spawnPoints[freeIndex];
        return true;
    }

    private List<int> GetFreeSpawnPointIndices()
    {
        List<int> freeIndices = new();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null)
                continue;

            bool isOccupied = false;

            foreach (int usedIndex in aliveSpawnPointIndices.Values)
            {
                if (usedIndex == i)
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                freeIndices.Add(i);
            }
        }

        return freeIndices;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 localOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
        return transform.position + localOffset;
    }

    public void NotifyEnemyDied(SpawnedEnemyInstance deadEnemy)
    {
        if (deadEnemy == null)
            return;

        aliveEnemies.Remove(deadEnemy);
        aliveSpawnPointIndices.Remove(deadEnemy);

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (aliveEnemies.Count < GetMaxAllowedAliveCount())
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