using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard3 : MonoBehaviour, IInteractible
{

    public GameObject[] enemies;
    GameObject mikael;
    public GameObject ammoRoom;
    private void Start()
    {
        mikael = GameObject.Find("Mikael Angeloskovich");
    }
    public void Interact()
    {

        mikael.GetComponent<Mikael>().state = 1;

        mikael.GetComponent<Mikael>().dialogueState = 0;
        mikael.GetComponent<Mikael>().UpdateHUD();

        ammoRoom.GetComponent<DoorController>().locked = false;
        foreach (GameObject enemy in enemies)
        {

            if (enemy != null) enemy.SetActive(true);

        }
        Destroy(this.gameObject);
    }
}