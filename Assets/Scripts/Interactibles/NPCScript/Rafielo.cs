using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rafielo : NPCBehavior
{
    //public GameObject nukeDoor;
    public GameObject keyCard;
    public GameObject keyCard2;

    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    // Update is called once per frame
    public override void UpdateHUD()
    {
        if (state == 0 && dialogueState == 9)
        {
            keyCard.SetActive(true);
        }

        if (state == 1 && dialogueState == 7)
        {

            //nukeDoor.GetComponent<DoorController>().Unlock();
            keyCard2.SetActive(true);


        }
        base.UpdateHUD();
    }
}