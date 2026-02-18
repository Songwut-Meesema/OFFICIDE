using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Sprite deadBody;
    public int maxHealth = 100;
    [SerializeField] float health;

    EnemyStates es;
    NavMeshAgent nma;
    SpriteRenderer sr;
    BoxCollider bc;
    Animator anim;

    public bool isBoss = false;
    public GameObject[] minionPrefab;
    public int numberOfMinions = 4;
    bool hasSpawnedMinions = false;
    bool canSpawnMinions = true;

    public GameObject healthItem;
    public GameObject ammoItem;
    public GameObject coffeeItem;
    bool hasDroppedItem = false;

    public GameObject victoryCanvas;

    // Boss Attack
    public GameObject missilePrefab;
    public float attackDamage = 20f;

    int currentPattern = 0;
    float patternTimer = 0f;

    private void Start()
    {
        if (isBoss) maxHealth = 1000;
        health = maxHealth;

        es = GetComponent<EnemyStates>();
        nma = GetComponent<NavMeshAgent>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (!hasDroppedItem)
            {
                hasDroppedItem = true;
                es.enabled = false;
                nma.enabled = false;
                sr.sprite = deadBody;
                bc.center = new Vector3(0, -0.8f, 0);
                bc.size = new Vector3(1.05f, 0.43f, 0.2f);

                DropItem();

                if (isBoss)
                {
                    Time.timeScale = 0f;
                    victoryCanvas.SetActive(true);
                }

                Destroy(gameObject, 2f);
            }
        }

        if (isBoss)
        {
            patternTimer += Time.deltaTime;

            if (patternTimer >= 4f) 
            {
                patternTimer = 0f;
                StartCoroutine(PerformPattern(currentPattern));
                currentPattern = (currentPattern + 1) % 3; 
            }

            if (!hasSpawnedMinions && health <= maxHealth * 0.5f && canSpawnMinions)
            {
                hasSpawnedMinions = true;
                SpawnMinions(Random.Range(2, 4));
            }
        }
    }

    IEnumerator PerformPattern(int index)
    {
        switch (index)
        {
            case 0:
                yield return StartCoroutine(ShootMissiles(3)); 
                break;
            case 1:
                yield return StartCoroutine(DashAttack()); 
                break;
            case 2:
                yield return StartCoroutine(ChargeAttack()); 
                break;
        }
    }

    IEnumerator DashAttack()
    {
        
        for (int i = 0; i < 5; i++)
        {
            sr.color = (i % 2 == 0) ? Color.blue : Color.yellow;
            yield return new WaitForSeconds(0.1f);
        }

        sr.color = Color.white;

     
        Vector3 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        float dashSpeed = 20f;
        float dashDuration = 0.3f;
        float time = 0f;

        
        while (time < dashDuration)
        {
            transform.position += direction * dashSpeed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ChargeAttack()
    {
        Vector3 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        float chargeSpeed = 15f;
        float chargeDistance = 10f;
        float distanceTravelled = 0f;

        while (distanceTravelled < chargeDistance)
        {
            transform.position += direction * chargeSpeed * Time.deltaTime;
            distanceTravelled += chargeSpeed * Time.deltaTime;
            yield return null;
        }
    }

    
    IEnumerator ShootMissiles(int count)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < count; i++)
        {
            if (missilePrefab != null && player != null)
            {
                
                Vector3 dir = (player.transform.position - transform.position).normalized;

                GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity); 

                missile.SendMessage("SetDamage", attackDamage, SendMessageOptions.DontRequireReceiver);

                missile.GetComponent<Rigidbody>().velocity = dir * 15f; 

                Destroy(missile, 1.5f);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void AddDamage(float damage)
    {
        health -= damage;
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    void DropItem()
    {
        float roll = Random.Range(0f, 1f);

        if (roll > 0.5f)
        {
            int itemIndex = Random.Range(0, 3);
            GameObject itemToDrop = null;

            switch (itemIndex)
            {
                case 0: itemToDrop = healthItem; break;
                case 1: itemToDrop = ammoItem; break;
                case 2: itemToDrop = coffeeItem; break;
            }

            if (itemToDrop != null)
            {
                Instantiate(itemToDrop, transform.position, Quaternion.identity);
            }
        }
    }

    void SpawnMinions(int count)
    {
        if (!canSpawnMinions) return;

        canSpawnMinions = false;

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count;
            Vector3 spawnPos = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 2;

            int randomIndex = Random.Range(0, minionPrefab.Length);
            Instantiate(minionPrefab[randomIndex], spawnPos, Quaternion.identity);
        }

        StartCoroutine(WaitBeforeNextSpawn());
    }

    IEnumerator WaitBeforeNextSpawn()
    {
        yield return new WaitForSeconds(3f);
        canSpawnMinions = true;
    }
}
