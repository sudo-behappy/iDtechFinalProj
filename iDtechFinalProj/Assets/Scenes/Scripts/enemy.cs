using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;




public class enemy : MonoBehaviour
{
    public float sightRad = 20;                //the spotting range
    public float turningRad = 2;               //when the patrol point is inside this range, go to the next one
    public float shootingRad = 15;             //when the enemy is this far from the player, start shooting 
    public float safeRad = 5;                 //the min distence between the player and the enemy
    public float fanDeg = 45f;                  //the enemy vision fanshape
    public Transform patrolList;                //the patroling route, stored as turning points
    public Transform player;                    //the player the enemy is finding
    

    Transform target;
    Renderer renderer;
    NavMeshAgent agent;
    ArrayList patrolNode = new ArrayList();     //operatable patrol node list


    int currentNode = 0;                        //index of current approaching node
    int state = 0;                              //current state    
    int health = 3;

    float cameraOffset;                         //the looking around degree

    bool ifInvunerable = false;
    bool spotted = false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        agent = GetComponent<NavMeshAgent>();
        //get every turning points inside the patrol route
        foreach(Transform child in patrolList)
        {
            patrolNode.Add(child);
        }
        //init the destination
        SetTargetNode(currentNode);
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate(state);

        //finding player
        Vector3 rayOrigion = transform.position;
        RaycastHit hit;
        //calculate the 2d vector pointing to the player from the enemy
        Vector3 pointingVect = player.position - transform.position;
        //find the local forward vector
        Vector3 localForwardVect = transform.forward;
        //put the vector into 2d
        localForwardVect.y = 0;
        pointingVect.y = 0;
        //find the angle
        float angle = Vector3.Angle(localForwardVect, pointingVect);
        print(Vector3.Distance(player.position, transform.position));
        if (Vector3.Distance(player.position, transform.position) <= sightRad && angle <= fanDeg + cameraOffset)
        {
            //raycasting
            Physics.Raycast(rayOrigion, pointingVect, out hit, sightRad);
            GameObject obj = hit.transform.gameObject;
            
            //if the ray hits the player
            if (obj.CompareTag("Player"))
            {
                //if it's within the shooting range
                if (Vector3.Distance(player.position, transform.position) <= shootingRad)
                {
                    state = 2;
                }
                else
                {
                    state = 1;
                }
            }
            else
            {
                state = 0;
            }
        }
        else
        {
            state = 0;
        }
    }
    //update the script
    void StateUpdate(int state)
    {
        switch (state)
        {
            case 0:
                //patrolling
                if (spotted)
                {
                    SetTargetNode(findClosestPoint());
                }
                spotted = false;
                agent.speed = 3.5f;
                agent.angularSpeed = 120;


                if (Vector3.Distance(target.position, transform.position) <= turningRad)
                {

                    SetTargetNode(++currentNode);
                }

                break;
            case 1:
                agent.speed = 7;
                agent.angularSpeed = 300;
                //walk to player
                spotted = true;
                if (Vector3.Distance(player.position, transform.position) <= safeRad)
                {
                    SetTargetNode(transform);
                    transform.LookAt(player);
                }
                else
                {
                    SetTargetNode(player);
                }
                break;
            case 2:
                //fire at the player
                spotted = true;
                agent.speed = 3.5f;
                agent.angularSpeed = 300;
                shoot();
                if (Vector3.Distance(player.position, transform.position) <= safeRad)
                {
                    SetTargetNode(transform);
                }
                else
                {
                    transform.LookAt(player);
                    SetTargetNode(player);
                }
                break;
        }
    }
    //set the patrol node the enemy will approach(fin)
    void SetTargetNode(int node)
    {
        target = (Transform)patrolNode[node % patrolNode.Count];
        agent.destination = target.position;
    }

    //set the object the enemy will approach(fin)
    void SetTargetNode(Transform node)
    {
        print(node);
        target = node;
        agent.destination = target.position;
    }

    Transform findClosestPoint()
    {
        Transform ans = (Transform)patrolNode[currentNode];
        float min = float.MaxValue;
        foreach(Transform i in patrolNode)
        {
            if(Vector3.Distance(i.position, transform.position) <= min)
            {
                min = Vector3.Distance(i.position, transform.position);
                ans = i;
            }
        }
        return ans;
    }

    public void damage()
    {
        if (!ifInvunerable)
        {
            health--;
            ifInvunerable = true;
            StartCoroutine(Flash());
        }
        else
        {
            StopCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        Color c = renderer.material.color;
        for (int i = 0; i < 20; i++)
        {
            c.a = (float)(10 - i % 5) / 10;
            renderer.material.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        ifInvunerable = false;
        c.a = 1;
        renderer.material.color = c;
    }
    

    void shoot()
    {
        player.GetComponent<Player_movement>().damage();
    }


}
