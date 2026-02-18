using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoodlePen : MonoBehaviour
{
    public Sprite idlePen;
    public Sprite attackPen;
    public float meleeDamage = 10f;
    public float meleeRange = 2f;
    public float attackCooldown = 0.5f; 
    public AudioClip attackSound;

    bool isAttacking = false;
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        isAttacking = false;
        GetComponent<SpriteRenderer>().sprite = idlePen;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        GetComponent<SpriteRenderer>().sprite = attackPen;
        source.PlayOneShot(attackSound);

        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, meleeRange))
        {
            hit.collider.gameObject.SendMessage("AddDamage", meleeDamage, SendMessageOptions.DontRequireReceiver);
        }

        yield return new WaitForSeconds(0.1f); 

        GetComponent<SpriteRenderer>().sprite = idlePen;

        yield return new WaitForSeconds(attackCooldown - 0.1f); 

        isAttacking = false;
    }
}