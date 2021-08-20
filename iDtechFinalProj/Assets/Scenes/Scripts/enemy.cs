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
    public float fanDeg = 45;                  //the enemy vision fanshape
    public Transform patrolList;                //the patroling route, stored as turning points
    public Transform player;                    //the player the enemy is finding
    

    Transform target;
    Renderer renderer;
    NavMeshAgent agent;
    ArrayList patrolNode = new ArrayList();     //operatable patrol node list
    Transform closest;                          //the closest patrol point to the enemy

    int currentNode = 0;                        //index of current approaching node
    int state = 0;                              //current state    

    bool spotted = false;
    bool patroling = true;
    bool shooting = false;

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
        print("State" + state.ToString());
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
        if (Vector3.Distance(player.position, transform.position) <= sightRad && angle <= fanDeg)
        {
            //raycasting
            Physics.Raycast(rayOrigion, pointingVect, out hit, sightRad);
            print(hit.transform.gameObject.name);
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

    void StateUpdate(int state)
    {
        switch (state)
        {
            case 0:
                //patrolling
                patroling = true;
                shooting = false;
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
                agent.angularSpeed = 240;
                //walk to player
                patroling = false;
                shooting = false;
                spotted = true;
                if(Vector3.Distance(player.position, transform.position) <= safeRad)
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
                patroling = false;
                shooting = true;
                spotted = true;
                agent.speed = 3.5f;
                agent.angularSpeed = 300;
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
                //TODO: shoot at the player
        }
    }
}
