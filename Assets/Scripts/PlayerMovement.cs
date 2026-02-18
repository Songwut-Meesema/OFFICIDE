using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    public float playerWalkingSpeed = 30f;
    public float playerRunningSpeed = 50f;
    public float jumpStrength = 20f;
    public float verticalRotationLimit = 80f;
    public AudioClip pickupSound;
    public FlashScreen flash;

    float forwardMovement;
    float sidewaysMovement;
    float originalWalkSpeed;
    float originalRunSpeed;
    bool isBurnoutSlowed = false;

    float verticalVelocity;
    float verticalRotation = 0;
    CharacterController cc;
    AudioSource source;
    PlayerHealth playerHealth;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();

        originalWalkSpeed = playerWalkingSpeed;
        originalRunSpeed = playerRunningSpeed;
    }

    void Update()
    {
        if (!isBurnoutSlowed && playerHealth.burnoutPercent >= 1f)
        {
            StartCoroutine(BurnoutSlowdown());
        }

        float horizontalRotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y");
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Movement
        if (cc.isGrounded)
        {
            forwardMovement = Input.GetAxis("Vertical") * playerWalkingSpeed;
            sidewaysMovement = Input.GetAxis("Horizontal") * playerWalkingSpeed;

            // Running
            if (Input.GetKey(KeyCode.LeftShift))
            {
                forwardMovement = Input.GetAxis("Vertical") * playerRunningSpeed;
                sidewaysMovement = Input.GetAxis("Horizontal") * playerRunningSpeed;
            }

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    DynamicCrosshair.spread = DynamicCrosshair.RUN_SPREAD;
                } 
                else
                {
                    DynamicCrosshair.spread = DynamicCrosshair.WALK_SPREAD;
                }
            }
        } 
        else
        {
            DynamicCrosshair.spread = DynamicCrosshair.JUMP_SPREAD;
        }

        // Gravity
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        // Jumping
        if (Input.GetButton("Jump") && cc.isGrounded)
        {
            verticalVelocity = jumpStrength;
        }

        Vector3 playerMovement = new Vector3(sidewaysMovement, verticalVelocity, forwardMovement);
        cc.Move(transform.rotation * playerMovement * Time.deltaTime);
    }

    IEnumerator BurnoutSlowdown()
    {
        isBurnoutSlowed = true;

        // Save current speed
        float oldWalkSpeed = playerWalkingSpeed;
        float oldRunSpeed = playerRunningSpeed;

        // Set slowdown speed
        playerWalkingSpeed = 10f;
        playerRunningSpeed = 10f;

        yield return new WaitForSeconds(3f);

        // Reset speed
        playerWalkingSpeed = originalWalkSpeed;
        playerRunningSpeed = originalRunSpeed;

        // Reset Burnout
        playerHealth.burnoutPercent = 0f;

        isBurnoutSlowed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HpBonus"))
        {
            playerHealth.AddHealth(20);
        } 
        else if (other.CompareTag("CoffeeBonus"))
        {
            playerHealth.AddCoffee();
        } 
        else if (other.CompareTag("AmmoBonus"))
        {
            Transform weaponsContainer = transform.Find("Weapons");
            
            if (weaponsContainer != null)
            {
                Pistol pistol = weaponsContainer.Find("FINGERGUN")?.GetComponent<Pistol>();
                if (pistol != null)
                {
                    pistol.AddAmmo(6);
                }

                RocketLauncher rocketLauncher = weaponsContainer.Find("RocketLauncher")?.GetComponent<RocketLauncher>();
                if (rocketLauncher != null)
                {
                    rocketLauncher.AddRockets(2);
                }
            }
        }

        if(other.CompareTag("HpBonus") || other.CompareTag("CoffeeBonus") || other.CompareTag("AmmoBonus"))
        {
            flash.PickedUpBonus();
            source.PlayOneShot(pickupSound);
            Destroy(other.gameObject);
        }
    }

    public void SetOverworkRageSpeed()
    {
        playerWalkingSpeed = originalWalkSpeed + 20f;
        playerRunningSpeed = originalRunSpeed + 20f;
    }
    public void SpeedBoost()
    {
        playerWalkingSpeed *= 1.2f;
        playerRunningSpeed *= 1.2f;
    }

    public void ResetSpeed()
    {
        playerWalkingSpeed = originalWalkSpeed;
        playerRunningSpeed = originalRunSpeed;
    }
}
