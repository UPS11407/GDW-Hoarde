using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pykelangelo : NPCBehavior
{
    private int initalKillCount;
    private void Start()
    {
        Startup();
    }

    private void Update()
    {
        if(state == 1)
        {
            /*if(player.getKillCount() - initalKillCount > 12){
                ChangeState();
            }*/
        }
    }

    public override void UpdateHUD()
    {
        base.UpdateHUD();
        if(state == 1)
        {
            //initalKillCount = player.getKillCount();
        }
    }
}
