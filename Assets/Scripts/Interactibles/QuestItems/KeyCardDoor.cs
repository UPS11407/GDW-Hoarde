using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardDoor : MonoBehaviour, IInteractible
{

    public GameObject[] doors;

    public void Interact()
    {



        foreach (GameObject door in doors)
        {

            door.GetComponent<DoorController>().Unlock();

        }
        Destroy(this.gameObject);
    }
}