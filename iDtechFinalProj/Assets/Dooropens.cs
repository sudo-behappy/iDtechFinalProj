using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooropens : MonoBehaviour
{
    private Animator ani;
    // Start is called before the first frame update
    void Start()
    {
      ani = GetComponent<Collider other >
       if (other.tag == "player") 
        {
            ani.SetBool("OPENED", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
