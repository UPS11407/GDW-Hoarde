using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardDoor : MonoBehaviour, IInteractible
{

    public GameObject[] doors;
    public int level;
    Inventory inventory;
    public bool isKeycard1 = false;

    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void Interact()
    {
        if (isKeycard1)
        {
            Destroy(doors[0].transform.parent.gameObject);
        }

        inventory.SetKeycardLevel(level);
        foreach (GameObject door in doors)
        {

            door.GetComponent<DoorController>().Unlock();
        }
        Destroy(this.gameObject);
    }
}