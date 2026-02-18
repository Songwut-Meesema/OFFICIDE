using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public int maxHealth = 750;
    private int currentHealth;

    public GameObject missilePrefab;
    public float missileSpeed = 10f;
    public float missileDamage = 20f;
    public float shootInterval = 0.5f; 
    private bool isDead = false;

    public Sprite deadBody; 
    private SpriteRenderer sr;
    private BoxCollider bc;

    private float flashDuration = 0.2f;
    private bool isFlashing = false;

    public GameObject victoryUI; 

    private void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();
        StartCoroutine(AttackPattern()); 
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isFlashing)
        {
            currentHealth -= damage;
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        isFlashing = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        sr.color = Color.white;
        isFlashing = false;
    }

    private void Die()
    {
        isDead = true;
        sr.sprite = deadBody;
        bc.enabled = false; 
        DropItems(); 
        ShowVictoryUI(); 
        Destroy(gameObject, 3f); 
    }

    void DropItems()
    {
        
    }

    void ShowVictoryUI()
    {
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }
    }

    private IEnumerator AttackPattern()
    {
        while (!isDead)
        {
            FireSpiralMissiles();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void FireSpiralMissiles()
    {
        float angleStep = 10f;
        for (float angle = 0f; angle < 360f; angle += angleStep)
        {
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle));
            FireMissile(direction);
        }
    }

    private void FireMissile(Vector3 direction)
    {
        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        rb.velocity = direction * missileSpeed;
        Missile missileScript = missile.GetComponent<Missile>();
        missileScript.damage = missileDamage;
    }
}
