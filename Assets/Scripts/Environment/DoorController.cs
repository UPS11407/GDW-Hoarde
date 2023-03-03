using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script for opening doors, just a simple on trigger, open the door, and on exit,
 leave. TWEAKS NEEDED: Bullets currently act as  a door opener*/
public class DoorController : MonoBehaviour, IInteractible
{
    Animator doorAnim;
    bool locked = false;
    bool queuedOpen;
    // Start is called before the first frame update
    void Start()
    {
        doorAnim = this.transform.parent.GetComponent<Animator>();
    }
    private void Update()
    {
        if (queuedOpen)
        {
            Open();
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

        Open();
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
        locked = true;
        gameObject.name = "LOCKED";
    }
    public void Unlock()
    {
        locked = false;
        gameObject.name = "Open Door";
    }
}
