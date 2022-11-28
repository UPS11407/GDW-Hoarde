using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractible
{
    int itemIndex;
    GameObject player;

    public void Interact()
    {
        if(player.GetComponent<WeaponInventory>().barrelUpgrades.shotgun && player.GetComponent<WeaponInventory>().gripUpgrades.fullAuto 
            && player.GetComponent<WeaponInventory>().ammoUpgrades.explosive && player.GetComponent<WeaponInventory>().magazineUpgrades.extended)
        {
            Debug.Log("All Upgrades Unlocked, Skipping Roll");
        }
        else
        {
            //more brainrot (need to rework for release game)
            while (true)
            {
                itemIndex = RollItem();

                if (itemIndex == 0 && !player.GetComponent<WeaponInventory>().barrelUpgrades.shotgun)
                {
                    player.GetComponent<WeaponInventory>().barrelUpgrades.shotgun = true;
                    Debug.Log("Rolled Shotgun");
                    break;
                }

                if (itemIndex == 1 && !player.GetComponent<WeaponInventory>().gripUpgrades.fullAuto)
                {
                    player.GetComponent<WeaponInventory>().gripUpgrades.fullAuto = true;
                    Debug.Log("Rolled FullAuto");
                    break;
                }

                if (itemIndex == 2 && !player.GetComponent<WeaponInventory>().ammoUpgrades.explosive)
                {
                    player.GetComponent<WeaponInventory>().ammoUpgrades.explosive = true;
                    Debug.Log("Rolled Explosive Ammo");
                    break;
                }

                if (itemIndex == 3 && !player.GetComponent<WeaponInventory>().magazineUpgrades.extended)
                {
                    player.GetComponent<WeaponInventory>().magazineUpgrades.extended = true;
                    Debug.Log("Rolled Extended Mag");
                    break;
                }

                Debug.LogWarning("Re-Rolling");
            }
        }
        
        Debug.Log("YOU GOT LIGMA");

        Destroy(gameObject);
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    int RollItem()
    {
        return Random.Range(0, 4);
    }
}
