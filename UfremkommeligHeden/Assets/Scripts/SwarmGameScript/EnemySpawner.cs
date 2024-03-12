using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyTypes; // Array of enemy prefabs
    public Transform[] spawnPoints; // Array of spawn points
    public Wave[] waves; // Array of waves
    public int spawnCount = 5; // Number of enemies to spawn at once

    private Transform playerTransform; // Player's transform

    [System.Serializable]
    public class Enemy
    {
        public GameObject prefab; // Prefab of the enemy
    }

    [System.Serializable]
    public class EnemyWave
    {
        public int enemyType; // Index of the enemy type in the enemyTypes array
        public int count; // Number of this enemy type to spawn
    }

    [System.Serializable]
    public class Wave
    {
        public int delay; // Delay before this wave starts
        public List<EnemyWave> enemyWaves; // List of enemy waves for this wave
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        foreach (Wave wave in waves)
        {
            yield return new WaitForSeconds(wave.delay);

            foreach (EnemyWave enemyWave in wave.enemyWaves)
            {
                int remainingEnemies = enemyWave.count;
                while (remainingEnemies > 0)
                {
                    int enemiesToSpawn = Mathf.Min(spawnCount, remainingEnemies);
                    for (int i = 0; i < enemiesToSpawn; i++)
                    {
                        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                        Instantiate(enemyTypes[enemyWave.enemyType].prefab, spawnPoint.position, Quaternion.identity);
                    }
                    remainingEnemies -= enemiesToSpawn;
                    yield return new WaitForSeconds(1f);
                }
            }
        }
    }
}