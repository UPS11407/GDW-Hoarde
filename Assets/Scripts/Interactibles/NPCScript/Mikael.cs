using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mikael : NPCBehavior
{
    public DoorController[] doorColliders;
    public GameObject[] enemiesGroup1;
    public GameObject[] enemiesGroup2;
    public Toolbox toolBox;

    public override void UpdateHUD()
    {
        base.UpdateHUD();

        if (state >= 2)
        {
            toolBox.obtainable = true;
            foreach (GameObject enemy in enemiesGroup1)
            {
                if (enemy != null) enemy.SetActive(true);
            }
        }

        if (state >= 4)
        {
            foreach (DoorController door in doorColliders)
            {
                door.Unlock();
            }


            foreach (GameObject enemy in enemiesGroup2)
            {
                if (enemy != null) enemy.SetActive(true);
            }
        }

        
    }
}
