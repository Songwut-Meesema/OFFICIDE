using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject victoryCanvas;
    private bool bossSpawned = false;

    void Update()
    {
        if (!bossSpawned)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                SpawnBoss();
            }
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        Enemy enemyComponent = boss.GetComponent<Enemy>();
        if (enemyComponent != null && enemyComponent.isBoss)
        {
            enemyComponent.victoryCanvas = victoryCanvas;
        }

        // Shake camera when boss appears
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            CameraShake shaker = mainCam.GetComponent<CameraShake>();
            if (shaker != null)
            {
                StartCoroutine(shaker.Shake(0.5f, 0.3f)); 
            }
        }

        bossSpawned = true;
        Debug.Log("Boss ได้เกิดแล้ว!");
    }
}
