using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject player;

    public Text[] texts;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        debugDisplay();
    }

    public void debugDisplay()
    {
        var playerScript = player.GetComponent<Player_movement>();

        texts[0].text = playerScript.stateGetter();
        texts[1].text = playerScript.canStandUp.ToString();
        texts[1].text = playerScript.canProne.ToString();
        texts[2].text = playerScript.climbGetter().ToString();
        texts[3].text = player.transform.position.x.ToString() + player.transform.position.y.ToString();
    }

    public void reset()
    {

    }
}
