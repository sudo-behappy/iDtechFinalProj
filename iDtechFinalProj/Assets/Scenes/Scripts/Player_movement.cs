using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{    
    //movement variables
    float forward = 0;

    //
    float rotation = 0;
    float vRotation = 0;

    //speed
    public float linearSpeed = 5f;
    public float angularSpeed = 5f;

    //forces
    public float jumpForce = 50f;

    public float lookAngle = 10f;

    public Transform camera;
    public Animator Animator;

    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotation = transform.rotation.y;
        vRotation = camera.rotation.x;
    }

    // Update is called once per frame
    void Update()
    {

        forward = Input.GetAxis("Vertical");

        rotation += Input.GetAxis("Mouse X") * angularSpeed;
        vRotation -= Input.GetAxis("Mouse Y") * angularSpeed;

        vRotation = Mathf.Clamp(vRotation, -lookAngle, lookAngle);

        transform.rotation = Quaternion.Euler(0, rotation, 0);
        camera.rotation = Quaternion.Euler(vRotation, rotation, 0);

        if (Input.GetKeyDown(KeyCode.Space) && GetComponentInChildren<jumpTrigger>().isOnGround)
        {
            rb.AddForce(Vector3.up * jumpForce);
            GetComponentInChildren<jumpTrigger>().isOnGround = false;
        }

        Animator.SetFloat("forward", forward);
        Animator.SetFloat("turn", Input.GetAxis("Mouse X"));
    }
}
