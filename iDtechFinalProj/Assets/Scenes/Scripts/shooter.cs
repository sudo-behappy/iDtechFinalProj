using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public Camera cam;
    public float range = 10f;
    int ammo = 5;

    void Start()
    {

    }


    public void Reload()
    {




    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            ammo--;
            anim.SetTrigger("PEW");

            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range))
            {
                GameObject hitObject = hit.transform.gameObject;
                Debug.Log(hitObject);
                if (hitObject.CompareTag("Enemy"))
                {

                    //hitObject.GetComponent<enemycontroler>().Damage();

                }



            }


        }
        else if (Input.GetKeyDown(KeyCode.R) || ammo == 0)
        {

            anim.SetTrigger("reload");


        }
    }
}