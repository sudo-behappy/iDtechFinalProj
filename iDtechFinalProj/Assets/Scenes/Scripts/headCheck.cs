using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headCheck : MonoBehaviour
{
    public bool canStandUp = true;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            canStandUp = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            canStandUp = false;
        }
    }
}
