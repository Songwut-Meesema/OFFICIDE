using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    public int patrolRange;
    public int attackRange;
    public int shootRange;
    public Transform vision;
    public float stayAlertTime;
    public float viewAngle;

    public GameObject missile;
    public float missileDamage;
    public float missileSpeed;

    public bool onlyMelee = false;
    public float meleeDamage;
    public float attackDelay;

    public LayerMask raycastMask;

    [Header("Boss Settings")]
    public bool isBoss = false;
    public float spiralShootInterval = 2f;
    public int spiralBulletCount = 12;
    [Range(0f, 5f)]
    public float missileSpawnDistance = 1.5f;

    [Header("Boss Attack Patterns")]
    public bool enableSpiral = true;
    public bool enableCircular = true;
    public bool enableWave = true;
    public bool enableRandomBurst = true;
    
    [Range(1, 10)]
    public int waveCount = 3;
    [Range(0.1f, 2f)]
    public float waveBetweenDelay = 0.5f;
    
    [Range(5, 30)]
    public int randomBurstCount = 15;
    
    float patternTimer = 0f;
    int currentPattern = 0;
    float waveTimer = 0f;
    int waveIndex = 0;
    bool isExecutingPattern = false;

    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public IEnemyAI currentState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public Vector3 lastKnownPosition;

    void Awake()
    {
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        chaseState = new ChaseState(this);
        patrolState = new PatrolState(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = patrolState;
        navMeshAgent.isStopped = false;
    }

    void Update()
    {
        currentState.UpdateActions();

        if (isBoss && IsBossActive())
        {
            HandleBossAttackPatterns();
        }
    }

    bool IsBossActive()
    {
        return currentState == attackState || currentState == chaseState;
    }

    void HandleBossAttackPatterns()
    {
        if (!isExecutingPattern)
        {
            patternTimer += Time.deltaTime;
            if (patternTimer >= spiralShootInterval)
            {
                patternTimer = 0f;
                
                SelectRandomAttackPattern();
            }
        }
        else if (currentPattern == 2) 
        {
            waveTimer += Time.deltaTime;
            if (waveTimer >= waveBetweenDelay)
            {
                waveTimer = 0f;
                FireCircular(waveIndex * 30f); 
                waveIndex++;
                
                if (waveIndex >= waveCount)
                {
                    isExecutingPattern = false;
                    waveIndex = 0;
                }
            }
        }
    }

    void SelectRandomAttackPattern()
    {
        System.Collections.Generic.List<int> availablePatterns = new System.Collections.Generic.List<int>();
        
        if (enableSpiral) availablePatterns.Add(0);
        if (enableCircular) availablePatterns.Add(1);
        if (enableWave) availablePatterns.Add(2);
        if (enableRandomBurst) availablePatterns.Add(3);
        
        if (availablePatterns.Count == 0)
        {
            FireSpiral();
            return;
        }
        
        currentPattern = availablePatterns[Random.Range(0, availablePatterns.Count)];
        
        switch (currentPattern)
        {
            case 0:
                FireSpiral();
                break;
            case 1:
                FireCircular();
                break;
            case 2:
                isExecutingPattern = true;
                waveIndex = 0;
                waveTimer = waveBetweenDelay; 
                break;
            case 3:
                FireRandomBurst();
                break;
        }
    }

    void OnTriggerEnter(Collider otherObj)
    {
        currentState.OnTriggerEnter(otherObj);
    }

    void HiddenShot(Vector3 shotPosition)
    {
        Debug.Log("Ktoś strzelił");
        lastKnownPosition = shotPosition;
        currentState = alertState;
    }

    public bool EnemySpotted()
    {
        Vector3 direction = GameObject.FindWithTag("Player").transform.position - transform.position;
        float angle = Vector3.Angle(direction, vision.forward);

        if (angle < viewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(vision.transform.position, direction.normalized, out hit, patrolRange, raycastMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    chaseTarget = hit.transform;
                    lastKnownPosition = hit.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    void FireSpiral()
    {
        Transform player = GameObject.FindWithTag("Player")?.transform;
        if (player == null) return;
        
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 missileSpawnPoint = transform.position + Vector3.up * 0.5f;
        StartCoroutine(PreAttackEffect(missileSpawnPoint, Color.red));

        for (int i = 0; i < spiralBulletCount; i++)
        {
            float angle = i * 360f / spiralBulletCount;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 spawnPoint = missileSpawnPoint + dir * missileSpawnDistance;
            
            GameObject missileObj = Instantiate(missile, spawnPoint, Quaternion.LookRotation(dir));
            SetupMissile(missileObj, dir, true);
        }
    }

    void FireCircular(float startAngleOffset = 0f)
    {
        Vector3 missileSpawnPoint = transform.position + Vector3.up * 0.5f;

        StartCoroutine(PreAttackEffect(missileSpawnPoint, Color.blue));

        for (int i = 0; i < spiralBulletCount; i++)
        {
            float angle = startAngleOffset + i * 360f / spiralBulletCount;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 spawnPoint = missileSpawnPoint + dir * missileSpawnDistance;
            
            GameObject missileObj = Instantiate(missile, spawnPoint, Quaternion.LookRotation(dir));
            SetupMissile(missileObj, dir, true);
        }
    }

    void FireRandomBurst()
    {
        Vector3 missileSpawnPoint = transform.position + Vector3.up * 0.5f;

        StartCoroutine(PreAttackEffect(missileSpawnPoint, Color.green));

        for (int i = 0; i < randomBurstCount; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 dir = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
            float distanceVariation = Random.Range(0.8f, 1.2f);
            Vector3 spawnPoint = missileSpawnPoint + dir * missileSpawnDistance * distanceVariation;
            GameObject missileObj = Instantiate(missile, spawnPoint, Quaternion.LookRotation(dir));
            SetupMissile(missileObj, dir, true);
        }
    }

    void SetupMissile(GameObject missileObj, Vector3 direction, bool isBossMissile = false)
    {
        Missile missileScript = missileObj.GetComponent<Missile>();
        if (missileScript != null)
        {
            missileScript.damage = missileDamage;
            missileScript.speed = missileSpeed;
            missileScript.isBossMissile = isBossMissile;
        }

        Rigidbody rb = missileObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * missileSpeed;
        }
    }

    IEnumerator PreAttackEffect(Vector3 center, Color color)
    {
        yield return new WaitForSeconds(0.1f);
    }
}