using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorcode : MonoBehaviour
{
    public Animator ani;


    // Start is called before the first frame update
    void Start()
    {

       
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "player")
        {
            ani.SetBool("letmeopen", true);
        }

    }
    void OnTriggerExit(Collider other)
    {

        if (other.tag == "player")
        {
            ani.SetBool("letmeopen", false);
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
