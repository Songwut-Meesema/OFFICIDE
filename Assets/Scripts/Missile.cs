using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public bool isBossMissile = false;
    

    Transform player;
    float timer = 0f;
    float missileLife;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (!isBossMissile)
        {
            missileLife = 15f;
            transform.LookAt(player);
        }
        else
        {
            missileLife = 2f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > missileLife)
            Destroy(this.gameObject);

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("EnemyHit", damage, SendMessageOptions.DontRequireReceiver);
        }
        Destroy(this.gameObject);
    }
}