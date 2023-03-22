using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellKeycard : MonoBehaviour, IInteractible
{
    GameObject cellDoor;
    public void Interact()
    {
        cellDoor.GetComponent<MikaelDoor>().enabled = false;
        cellDoor.GetComponent<DoorController>().enabled = true;
        Destroy(this);

    }
}
