using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardDoor : MonoBehaviour, IInteractible
{

    public GameObject[] doors;
    public int level;
    Inventory inventory;

    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void Interact()
    {

        inventory.SetKeycardLevel(level);
        foreach (GameObject door in doors)
        {

            door.GetComponent<DoorController>().Unlock();
        }
        Destroy(this.gameObject);
    }
}