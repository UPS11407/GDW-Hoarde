using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    public DoorController door;
    // Start is called before the first frame update
    void Start()
    {
        door = this.transform.GetComponentInChildren<DoorController>();
    }

    // Update is called once per frame
    public void Interact()
    {
        door.Open();
    }
}
