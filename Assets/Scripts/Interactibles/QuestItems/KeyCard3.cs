using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard3 : MonoBehaviour, IInteractible
{

    public GameObject[] enemies;
    GameObject mikael;
    public GameObject ammoRoom;
    Inventory inventory;
    public int level;

    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        mikael = GameObject.Find("Mikael Angeloskovich");
    }
    public void Interact()
    {

        mikael.GetComponent<Mikael>().state = 1;

        mikael.GetComponent<Mikael>().dialogueState = 0;
        mikael.GetComponent<Mikael>().UpdateHUD();

        ammoRoom.GetComponent<DoorController>().locked = false;
        inventory.SetKeycardLevel(level);
        foreach (GameObject enemy in enemies)
        {

            if (enemy != null) enemy.SetActive(true);

        }
        Destroy(this.gameObject);
    }
}