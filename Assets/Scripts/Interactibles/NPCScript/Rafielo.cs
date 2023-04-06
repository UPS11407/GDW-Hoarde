using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rafielo : NPCBehavior
{
    //public GameObject nukeDoor;
    public GameObject keyCard;
    public GameObject keyCard2;
    public GameObject rifleCrate;

    // Start is called before the first frame update
    void Start()
    {
        Startup();
        rifleCrate.layer = 0;
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
            rifleCrate.layer = 8;

        }
        base.UpdateHUD();
    }
}