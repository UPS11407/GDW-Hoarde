using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script for opening doors, just a simple on trigger, open the door, and on exit,
 leave. TWEAKS NEEDED: Bullets currently act as  a door opener*/
public class DoorController : MonoBehaviour, IInteractible
{
    Animator doorAnim;
    public bool locked = false;
    bool queuedOpen;

    [SerializeField] float lockDuration;
    [SerializeField] float lockCooldown;
    float lockTime;
    public enum LockState { lockable, locked, charging }
    public LockState lockState;
    public bool isInteractable = true;

    // Start is called before the first frame update
    void Start()
    {
        doorAnim = this.transform.parent.GetComponent<Animator>();
        lockTime = -lockCooldown;
    }
    private void Update()
    {
        if (queuedOpen)
        {
            Open();
        }
        if (Time.time > lockTime + lockCooldown)
        {
            lockState = LockState.lockable;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Enemy") && !locked)
        {
            doorAnim.SetBool("isOpening", true);
        } else if ((other.tag.Equals("Enemy") && locked))
        {
            queuedOpen = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Enemy") || other.tag.Equals("Player"))
            doorAnim.SetBool("isOpening", false);
    }
    public void Interact()
    {
        if (isInteractable) Open();
    }
    public void Open()
    {
        if (!locked)
        {
            queuedOpen = false;
            doorAnim.SetBool("isOpening", true);
        }
        
    }
    public void Lock()
    {
        if (Time.time > lockTime + lockCooldown)
        {

            lockTime = Time.time;
            gameObject.name = "LOCKED";
            StartCoroutine(LockDoor());

        }
        
        

        
    }

    public void ForceLock()
    {
        gameObject.name = "LOCKED";
        locked = true;
        lockState = LockState.locked;
    }

    public void Unlock()
    {
        locked = false;
        gameObject.name = "Open Door";
        lockState = LockState.charging;
    }

    IEnumerator LockDoor()
    {
       
        locked = true;
        lockState = LockState.locked;
        yield return new WaitForSeconds(lockDuration);
        Unlock();
    }
}
