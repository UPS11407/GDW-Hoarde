using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leo : NPCBehavior
{
    public GameObject upperFloorDoor;
    public GameObject toolBox;
    public GameObject keyCard;

    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    // Update is called once per frame
    public override void UpdateHUD()
    {
        if (state == 0 && dialogueState == 8)
        {
            toolBox.SetActive(true);
        }
        if (state == 0 && dialogueState == 1)
        {
            player.GetComponent<Player>().TakeDamage(1, "Thrown laptop");
        }
        if (state == 1 && dialogueState == 6)
        {
            player.GetComponent<Player>().hasNV = true;
            keyCard.SetActive(true);

        }

        base.UpdateHUD();
    }
}
