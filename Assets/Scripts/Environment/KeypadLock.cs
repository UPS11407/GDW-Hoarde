using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadLock : MonoBehaviour, IInteractible
{
    [SerializeField] DoorController door;
    private void Start()
    {
        gameObject.name = "Keypad Lock";
    }


    public void Interact()
    {
        if(door.lockState == DoorController.LockState.locked)
        {
            door.Unlock();
        } else
        {

            door.Lock();
        }
    }
    
}
