using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadLock : MonoBehaviour, IInteractible
{
    [SerializeField] DoorController door;
    [SerializeField] Color lockedColor;
    [SerializeField] Color lockableColor;
    [SerializeField] Color chargingColor;
    [SerializeField] Renderer LCD;
    private void Start()
    {
        gameObject.name = "Keypad Lock";
    }
    private void Update()
    {
        if (door.lockState == DoorController.LockState.locked)
        {
            LCD.material.color = lockedColor;
        } else if (door.lockState == DoorController.LockState.lockable)
        {
            LCD.material.color = lockableColor;
        } else
        {
            LCD.material.color = chargingColor;

        }
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
