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

    public Transform camera;

    int health = 5;
    bool ifInvunerable = true;
    float invunerableTimer;
    public const float INVUNERABLE_TIME = 5;

    Rigidbody rb;


    void Start()
    {
        rotation = transform.rotation.y;
        vRotation = camera.GetComponent<Transform>().rotation.x;
        rb = GetComponent<Rigidbody>();
        state = 0;
        for(int i =0; i < bodyList.Length; i++)
        {
            bodyList[i].SetActive(false);
        }
        bodyList[1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (ifInvunerable)
        {
            invunerableTimer += Time.deltaTime;
            if(invunerableTimer >= INVUNERABLE_TIME)
            {
                ifInvunerable = false;
                invunerableTimer = 0;
            }
        }
        print(health);
        print(ifInvunerable);
        if(health <= 0)
        {
            print("DIE");
        }

        if (stateFlag)
        {
            bodyList[state % 3].SetActive(false);
            bodyList[(state + 1) % 3].SetActive(true);
        }
        forward = Input.GetAxis("Vertical");

        rotation += Input.GetAxis("Mouse X") * angularSpeed;
        vRotation -= Input.GetAxis("Mouse Y") * angularSpeed;
        vRotation = Mathf.Clamp(vRotation, -lookAngle, lookAngle);

        transform.rotation = Quaternion.Euler(0, rotation, 0);
        camera.GetComponent<Transform>().rotation = Quaternion.Euler(vRotation, rotation, 0);
        //the first time start from 0

        if (Input.GetKeyDown(KeyCode.C) && stateChangeChecker())
        {
            //next state
            state++;
            stateFlag = true;
        }
    }

    void FixedUpdate()
    {
        //movement
        Vector3 motion = transform.forward * forward * linearSpeed;
        motion.y = rb.velocity.y;
        rb.velocity = motion;
        
    }

    

    //state getter
    public string stateGetter()
    {
        return (state % 3).ToString();
    }



    public void damage()
    {
        if (!ifInvunerable)
        {
            print("----------------------------");
            print("Hit");
            print("Health: " + health.ToString());
            print("----------------------------");
            health--;
            ifInvunerable = true;
        }
        
    }

    bool stateChangeChecker()
    {
        Collider collider = bodyList[(state + 1) % 3].GetComponent<CapsuleCollider>();
        collider.isTrigger = true;
        bool ans = collider.gameObject.GetComponent<bodyCollider>().ifCanChange;
        collider.isTrigger = false;
        return ans;
    }
}
