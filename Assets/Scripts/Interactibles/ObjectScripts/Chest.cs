using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractible
{
    int itemIndex;
    GameObject player;
    bool canInteract = true;
    Animator animator;
    public GameObject menuText;
    public bool tutorialChest;


    public void Interact()
    {
        if (canInteract)
        {
            animator.Play("Open");
            canInteract = false;
            if (tutorialChest)
            {
                GameObject.Find("Pistol").GetComponent<Gun>().canSwap = true;
                player.GetComponent<WeaponManager>().SwapWeapon();
                GameObject.Find("Johnatelo").GetComponent<NPCBehavior>().state = 2;
            }
            else
            {
                if (player.GetComponent<WeaponInventory>().barrelUpgrades.shotgun && player.GetComponent<WeaponInventory>().gripUpgrades.fullAuto
                    && player.GetComponent<WeaponInventory>().ammoUpgrades.explosive && player.GetComponent<WeaponInventory>().magazineUpgrades.extended 
                    && player.GetComponent<WeaponInventory>().barrelUpgrades.sniper && player.GetComponent<WeaponInventory>().magazineUpgrades.quick)
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

                        if (itemIndex == 4 && !player.GetComponent<WeaponInventory>().magazineUpgrades.quick)
                        {
                            player.GetComponent<WeaponInventory>().magazineUpgrades.quick = true;
                            Debug.Log("Rolled Quick Mag");
                            break;
                        }

                        if (itemIndex == 5 && !player.GetComponent<WeaponInventory>().barrelUpgrades.sniper)
                        {
                            player.GetComponent<WeaponInventory>().barrelUpgrades.sniper = true;
                            Debug.Log("Rolled Sniper Barrel");
                            break;
                        }

                        Debug.LogWarning("Re-Rolling");
                    }
                }

                StartCoroutine(ModMenuText());
            }
            Debug.Log("YOU GOT LIGMA");
            
            Destroy(gameObject, 6);
            
        }
    }

    IEnumerator ModMenuText()
    {
        menuText.SetActive(true);
        yield return new WaitForSeconds(5.9f);
        menuText.SetActive(false);
    }

    private void Awake()
    {
        //menuText = GameObject.Find("ChestText");
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

    int RollItem()
    {
        return Random.Range(0, 4);
    }
}
