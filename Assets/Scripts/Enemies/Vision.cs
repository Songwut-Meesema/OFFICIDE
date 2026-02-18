using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vision : MonoBehaviour
{

    Vector3 destination;

    void Update()
    {
        destination = transform.parent.GetComponent<EnemyStates>().navMeshAgent.destination;
        transform.LookAt(destination);
    }
}