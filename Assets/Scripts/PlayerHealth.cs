using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public AudioClip hit;
    public AudioClip coffeeDrinkSound;
    public FlashScreen flash;
    
    AudioSource source;
    bool isGameOver = false;
    
    [SerializeField] float health;
    [SerializeField] float burnoutLevel = 0f;
    [SerializeField] int coffeeCount = 0;
    [SerializeField] float overworkRageTimer = 0f;
    
    const float MAX_BURNOUT = 100f;
    const float COFFEE_BURNOUT_REDUCTION = 30f;
    const float OVERWORK_RAGE_DURATION = 5f;
    const float SPEED_PENALTY = 0.2f;

    
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI burnoutText;
    public Image[] coffeeIcons; 
    public Image burnoutBar;

    void Start()
    {
        health = 100f;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        health = Mathf.Clamp(health, 0, 100f);
        burnoutLevel = Mathf.Clamp(burnoutLevel, 0, MAX_BURNOUT);

       
        UpdateHealthUI();
        UpdateBurnoutUI();
        UpdateCoffeeUI();

       
        if (overworkRageTimer > 0)
        {
            overworkRageTimer -= Time.deltaTime;
            if (overworkRageTimer <= 0)
            {
                EndOverworkRage();
            }
        }

        
        if (health <= 0 && !isGameOver)
        {
            isGameOver = true;
            GameManager.Instance.PlayerDeath(); 
        }

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseCoffee();
        }
    }

    void UseCoffee()
    {
        if (coffeeCount <= 0) return;

        source.PlayOneShot(coffeeDrinkSound);

        if (coffeeCount >= 3 && overworkRageTimer <= 0)
        {
       
        coffeeCount = 0;
        burnoutLevel = 0f;
        StartOverworkRage();
        }
    else
        {
        
        coffeeCount--;
        burnoutLevel -= 20f;
        burnoutLevel = Mathf.Clamp(burnoutLevel, 0f, MAX_BURNOUT);
        }
    }

    void StartOverworkRage()
{
    overworkRageTimer = OVERWORK_RAGE_DURATION;

    
    PlayerMovement pm = GetComponent<PlayerMovement>();
    if (pm != null)
    {
        pm.SetOverworkRageSpeed();
    }
}

void EndOverworkRage()
{
    PlayerMovement pm = GetComponent<PlayerMovement>();
    if (pm != null)
    {
        pm.ResetSpeed();
    }
}

    public void AddHealth(float value)
    {
        health = Mathf.Min(health + value, 100f);
    }

    void EnemyHit(float damage)
    {
        health -= damage;
        burnoutLevel += damage;
        source.PlayOneShot(hit);        
        flash.TookDamage();
    }

    public void AddCoffee()
    {
        if (coffeeCount < 3)
        {
            coffeeCount++;
        }
    }

    public float burnoutPercent
    {
        get { return burnoutLevel / MAX_BURNOUT; }
        set { burnoutLevel = Mathf.Clamp(value * MAX_BURNOUT, 0, MAX_BURNOUT); }
    }

    
    void UpdateHealthUI()
    {
        healthText.text = $"{health}%";
    }

    void UpdateBurnoutUI()
    {
        burnoutText.text = $"{burnoutLevel}%";
        burnoutBar.fillAmount = burnoutLevel / MAX_BURNOUT; 
    }

    void UpdateCoffeeUI()
    {
        for (int i = 0; i < coffeeIcons.Length; i++)
        {
            if (i < coffeeCount)
            {
                coffeeIcons[i].enabled = true; 
            }
            else
            {
                coffeeIcons[i].enabled = false; 
            }
        }
    }
}
