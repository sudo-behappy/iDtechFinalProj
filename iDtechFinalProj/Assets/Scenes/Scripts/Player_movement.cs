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
    public float linearSpeed = 15f;
    public float angularSpeed = 20f;

    //movement
    public bool canStandUp = true;
    public bool canProne = true;

    //possible states
    public GameObject[] bodyList;

    //playerState
    int state = 0;
    string[] stateList = { "Stand", "Crouch", "Prone" };
    bool stateFlag = false;

    //sensitivity
    public float lookAngle = 50f;

    Camera[] cameraList = new Camera[3];


    Rigidbody rb;
    Transform ts;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ts = GetComponent<Transform>();
        state = 0;
        for(int i = 1; i < bodyList.Length; i++)
        {
            bodyList[i].SetActive(false);
            cameraList[i] = bodyList[i].GetComponent<Camera>();
            cameraList[i].enabled = false;
        }
        cameraList[0].enabled = true;
        rotation = transform.rotation.y;
        vRotation = cameraList[state % 3].GetComponent<Transform>().rotation.x;
    }

    // Update is called once per frame
    void Update()
    {

        if (stateFlag)
        {
            rotation = transform.rotation.y;
            vRotation = cameraList[state % 3].GetComponent<Transform>().rotation.x;
            bodyList[state % 3].SetActive(false);
            bodyList[(state + 1) % 3].SetActive(true);
            cameraList[state % 3].enabled = false;
            cameraList[(state + 1) % 3].enabled = true;
        }
        forward = Input.GetAxis("Vertical");

        rotation += Input.GetAxis("Mouse X") * angularSpeed;
        vRotation -= Input.GetAxis("Mouse Y") * angularSpeed;
        vRotation = Mathf.Clamp(vRotation, -lookAngle, lookAngle);

        transform.rotation = Quaternion.Euler(0, rotation, 0);
        cameraList[state % 3].GetComponent<Transform>().rotation = Quaternion.Euler(vRotation, rotation, 0);

        //Animator.SetFloat("forward", forward);
        //Animator.SetFloat("turn", Input.GetAxis("Mouse X"));
        
        //the first time start from 0

        if (Input.GetKeyDown(KeyCode.C))
        {
            //next state
            state++;
            stateFlag = true;
        }

        print(stateChangeChecker());
    }

    void FixedUpdate()
    {
        //movement
        Vector3 motion = transform.forward * forward * linearSpeed;
        //motion += transform.forward * left * linearSpeed;
        motion.y = rb.velocity.y;
        rb.velocity = motion;
        
    }



    //state getter
    public string stateGetter()
    {
        return (state % 3).ToString();
    }

    //climb object getter
    public GameObject climbGetter()
    {
        return null;
    }

    bool stateChangeChecker()
    {
        Collider collider = bodyList[state % 3].GetComponent<CapsuleCollider>();
        collider.isTrigger = true;
        bool ans = collider.gameObject.GetComponent<bodyCollider>().ifCanChange;
        collider.isTrigger = false;
        return ans;
    }
}
