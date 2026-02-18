using UnityEngine;

public class BossInitializer : MonoBehaviour
{
    void Start()
    {
        Enemy enemy = GetComponent<Enemy>();

        if (enemy.isBoss)
        {
            GameObject baldZombie = Resources.Load<GameObject>("BaldZombie");
            GameObject zombieOffice = Resources.Load<GameObject>("ZombieOffice");

            if (baldZombie != null && zombieOffice != null)
            {
                enemy.minionPrefab = new GameObject[] { baldZombie, zombieOffice };
            }
            else
            {
                Debug.LogWarning("not found prefabs");
            }
            
            GameObject victoryUI = GameObject.FindGameObjectWithTag("VictoryCanvas");

            if (victoryUI != null)
            {
                enemy.victoryCanvas = victoryUI;
            }
            else
            {
                Debug.LogWarning("canvas not found");
            }
        }
    }
}
