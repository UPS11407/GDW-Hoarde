using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mikael : NPCBehavior
{
    public GameObject[] armoryDoors;
    public GameObject keyCard;

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
        
        if (state == 1 && dialogueState == 3)
        {
            foreach (GameObject door in armoryDoors) 
            {
                door.GetComponent<DoorController>().Unlock();

            }
        }
        base.UpdateHUD();
    }
}