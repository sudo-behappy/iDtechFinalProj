using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorcode : MonoBehaviour
{
    private Animator ani;


    // Start is called before the first frame update
    void Start()
    {

        ani = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "player")
        {
            ani.SetBool("letmeopen", true);
        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
