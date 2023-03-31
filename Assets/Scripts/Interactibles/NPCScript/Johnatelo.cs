using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Johnatelo : NPCBehavior
{
    public DoorController genDoor;
    public DoorController[] doorColliders;
    public GameObject[] enemies;
    public WeaponManager weaponManager;

    void Start()
    {
        Startup();
        genDoor.ForceLock();
        foreach (DoorController door in doorColliders)
        {
            door.ForceLock();

        }

        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    public override void UpdateHUD()
    {
        base.UpdateHUD();

        if (state == 0 && !weaponManager.gunInventory.Contains(1) && dialogueState == 3)
        {
            weaponManager.gunInventory.Add(1);
            player.GetComponent<WeaponManager>().DoWeaponSwap(player.GetComponent<WeaponManager>().activeGun, 1);

            foreach (DoorController door in doorColliders)
            {
                door.Unlock();
            }


            foreach (GameObject enemy in enemies)
            {
                if (enemy != null) enemy.SetActive(true);
            }

            Debug.Log("YEET");
        }

        if (state >= 3)
        {
            genDoor.Unlock();
        }
    }
}