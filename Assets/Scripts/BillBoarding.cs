using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
       
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(cameraTransform.position);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f); 
    }
}
