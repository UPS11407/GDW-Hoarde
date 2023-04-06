using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Johnatelo : NPCBehavior
{
    public DoorController genDoor;
    public DoorController[] doorColliders;
    public GameObject[] enemies;
    public WeaponManager weaponManager;
    GameObject inventoryText;

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

        inventoryText = GameObject.Find("InventoryText");
    }

    public override void UpdateHUD()
    {
        

        if (state == 0 && dialogueState == 3)
        {
            if (!weaponManager.gunInventory.Contains(1)) weaponManager.gunInventory.Add(1);
            player.GetComponent<WeaponManager>().DoWeaponSwap(player.GetComponent<WeaponManager>().activeGun, 1);
            foreach (DoorController door in doorColliders)
            {
                door.Unlock();
                StartCoroutine(InventoryText($"Press {GameObject.Find("Player").GetComponent<Player>().GetBindingReadable(2)} for Flashlight", Color.yellow));
            }


            foreach (GameObject enemy in enemies)
            {
                if (enemy != null) enemy.SetActive(true);
            }
        }

        if (state >= 1 && dialogueState == 2)
        {
            genDoor.Unlock();
        }

        base.UpdateHUD();

    }

    IEnumerator InventoryText(string text, Color color)
    {
        inventoryText.transform.GetChild(0).gameObject.SetActive(true);
        inventoryText.transform.GetChild(1).gameObject.SetActive(true);

        inventoryText.SetActive(true);
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().color = color;

        yield return new WaitForSeconds(4.9f);

        inventoryText.transform.GetChild(0).gameObject.SetActive(false);
        inventoryText.transform.GetChild(1).gameObject.SetActive(false);
    }
}