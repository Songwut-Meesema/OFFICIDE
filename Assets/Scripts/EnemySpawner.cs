using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; 
    public int maxEnemies = 5; 
    public float spawnRadius = 5f; 

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0) return;

        for (int i = 0; i < maxEnemies; i++)
        {
            Vector3 spawnPosition = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
        }
    }
}
