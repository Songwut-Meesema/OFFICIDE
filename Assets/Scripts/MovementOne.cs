using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMentOne : MonoBehaviour
{
    public float speed = 10f;
    public float momentumDamping = 5f;

    private CharacterController characterController;
    public Animator camAnim;
    private bool isWalking;

    private Vector3 inputVector;
    private Vector3 movementVector;
    private float myGravity = -10f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();  
    }

    void Update()
    {
        PlayerInput();
        MoveMent();

        camAnim.SetBool("isWalking",isWalking);
    }

    void PlayerInput()
    {
        if(Input.GetKey(KeyCode.W) ||
           Input.GetKey(KeyCode.A) ||
           Input.GetKey(KeyCode.S) ||
           Input.GetKey(KeyCode.D))
        {

            inputVector = new Vector3(x: Input.GetAxisRaw("Horizontal"), y: 0f, z: Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);

            isWalking = true;

        }
        else
        {
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);

            isWalking= false;
        }
        
        movementVector = (inputVector * speed) + (Vector3.up * myGravity);
    }

    void MoveMent()
    {
        characterController.Move(movementVector * Time.deltaTime);
    }
}
