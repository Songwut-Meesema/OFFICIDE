using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Pistol : MonoBehaviour
{
    public GameObject bloodSplat;
    public Sprite idlePistol;
    public Sprite shotPistol;
    public float pistolDamage;
    public float pistolRange;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public AudioClip emptyGunSound;

    public Text ammoText;

    public int ammoAmount;
    public int ammoClipSize;

    public GameObject bulletHole;

    int ammoLeft;
    int ammoClipLeft;

    bool isShot;
    bool isReloading;
    bool isReloadingUIUpdateBlocked = false; 

    AudioSource source;

    public float fireRate = 0.5f;  
    private float lastShotTime = 0f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        ammoLeft = ammoAmount;
        ammoClipLeft = ammoClipSize;
    }

    void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        if (!isReloadingUIUpdateBlocked) 
        {
            ammoText.text = ammoClipLeft + " / " + ammoLeft;
        }

        if (Input.GetButtonDown("Fire1") && isReloading == false && ammoClipLeft > 0 && Time.time >= lastShotTime + fireRate)
        {
            isShot = true;  
        }

        if (ammoClipLeft == 0 && !isReloading)
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.R) && isReloading == false && ammoClipLeft != ammoClipSize)
        {
            Reload();
        }
    }

    void FixedUpdate()
    {
        Vector2 bulletOffset = Random.insideUnitCircle * DynamicCrosshair.spread;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + bulletOffset.x, Screen.height / 2 + bulletOffset.y, 0));
        RaycastHit hit;

        if (isShot && ammoClipLeft > 0 && isReloading == false)
        {
            isShot = false; 
            DynamicCrosshair.spread += DynamicCrosshair.PISTOL_SHOOTING_SPREAD;
            ammoClipLeft--;
            source.PlayOneShot(shotSound);
            StartCoroutine("shot");

            if (Physics.Raycast(ray, out hit, pistolRange))
            {
                Debug.Log("ยิงๆ" + hit.collider.gameObject.name);
                hit.collider.gameObject.SendMessage("AddDamage", pistolDamage, SendMessageOptions.DontRequireReceiver);
                if (hit.transform.CompareTag("Enemy"))
                {
                    Instantiate(bloodSplat, hit.point, Quaternion.identity);
                    if (hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().patrolState ||
                        hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().alertState)
                        hit.collider.gameObject.SendMessage("HiddenShot", transform.parent.transform.position, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).transform.parent =
                        hit.collider.gameObject.transform;
                }
            }

            lastShotTime = Time.time; 
        }
        else if (isShot && ammoClipLeft <= 0 && isReloading == false)
        {
            isShot = false;
            Reload();
        }
    }

    void Reload()
    {
        int bulletsToReload = ammoClipSize - ammoClipLeft;
        if (ammoLeft >= bulletsToReload)
        {
            StartCoroutine("ReloadWeapon");
            ammoLeft -= bulletsToReload;
            ammoClipLeft = ammoClipSize;
        }
        else if (ammoLeft < bulletsToReload && ammoLeft > 0)
        {
            StartCoroutine("ReloadWeapon");
            ammoClipLeft += ammoLeft;
            ammoLeft = 0;
        }
        else if (ammoLeft <= 0)
        {
            source.PlayOneShot(emptyGunSound);
        }
    }

    IEnumerator ReloadWeapon()
    {
        isReloading = true;
        isReloadingUIUpdateBlocked = true; 
        source.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(0.35f); 
        isReloading = false;
        isReloadingUIUpdateBlocked = false; 
    }

    IEnumerator shot()
    {
        GetComponent<SpriteRenderer>().sprite = shotPistol;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = idlePistol;
    }

    public void AddAmmo(int value)
    {
        ammoLeft += value;
    }
}
