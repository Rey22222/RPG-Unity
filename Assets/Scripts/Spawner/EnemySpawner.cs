using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints;
    public List<GameObject> enemyPrefabs;

    public int maxEnemies = 5;
    public float spawnInterval = 90f;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            activeEnemies.RemoveAll(enemy => enemy == null);

            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0 || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No spawn points or enemy prefabs assigned.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        GameObject newEnemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        activeEnemies.Add(newEnemy);
    }
}
