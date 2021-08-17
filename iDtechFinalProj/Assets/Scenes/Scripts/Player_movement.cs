using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{    
    //movement variables
    float forward = 0;
    float rotation = 0;
    float vRotation = 0;

    //speed
    public float linearSpeed = 5f;
    public float angularSpeed = 20f;

    //playerState
    int state = 0;
    string[] stateList = { "Stand", "Crouch", "Prone" };
    bool canStandUp = true;

    //sensitivity
    public float lookAngle = 50f;

    public Transform camera;
    public Animator Animator;



    Rigidbody rb;
    Transform ts;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ts = GetComponent<Transform>();
        rotation = transform.rotation.y;
        vRotation = camera.rotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        canStandUp = GetComponentInChildren<headCheck>().canStandUp;
        forward = Input.GetAxis("Vertical");

        rotation += Input.GetAxis("Mouse X") * angularSpeed;
        vRotation -= Input.GetAxis("Mouse Y") * angularSpeed;

        vRotation = Mathf.Clamp(vRotation, -lookAngle, lookAngle);

        transform.rotation = Quaternion.Euler(0, rotation, 0);
        camera.rotation = Quaternion.Euler(vRotation, rotation, 0);

        Animator.SetFloat("forward", forward);
        Animator.SetFloat("turn", Input.GetAxis("Mouse X"));

        if (Input.GetKeyDown(KeyCode.C))
        {
            state += 1;
        }
    }

    void FixedUpdate()
    {
        //movement
        Vector3 motion = transform.forward * forward * linearSpeed;
        //motion += transform.forward * left * linearSpeed;
        motion.y = rb.velocity.y;
        rb.velocity = motion;
    }

    void stateUpdate()
    {
        state = 1;
        print(stateList[state]);
        switch (state % 3)
        {
            case 0:
                //normal standing
                if ()
                {
                    ts.localScale = new Vector3(1, 1.5f, 1);
                    break;
                }
            case 1:
                //crouch
                ts.localScale = new Vector3(1, 1, 1);
                break;
            case 2:
                //prone
                ts.localScale = new Vector3(1, 1.5f, 1);
                ts.localEulerAngles = new Vector3(90, ts.localEulerAngles.y, 0);
                break;
        }

        
    }
}
