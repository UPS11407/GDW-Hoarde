using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadLock : MonoBehaviour, IInteractible
{
    [SerializeField] DoorController door;
    [SerializeField] float lockDuration;
    [SerializeField] float lockCooldown;
    float lockTime;
    public enum LockState { lockable, locked, charging}
    public LockState lockState;
    private void Start()
    {
        lockTime = -lockCooldown;
    }
    private void Update()
    {
        if (Time.time > lockTime + lockCooldown)
        {
            lockState = LockState.lockable;
        }
    }
    public void Interact()
    {
        Debug.Log("Interact");
        Debug.Log($"Time.time: {Time.time}, and Time+CD: {lockTime + lockCooldown}");
        if (Time.time > lockTime + lockCooldown)
        {
            
            lockTime = Time.time;
            StartCoroutine(Lock());

        }
    }
    IEnumerator Lock()
    {
        Debug.Log("Lock");
        door.Lock();
        lockState = LockState.locked;
        yield return new WaitForSeconds(lockDuration);
        door.Unlock();
        Debug.Log("UnLock");
        lockState = LockState.charging;
    }
}
