using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script for opening doors, just a simple on trigger, open the door, and on exit,
 leave. TWEAKS NEEDED: Bullets currently act as  a door opener*/
public class DoorController : MonoBehaviour, IInteractible
{
    Animator doorAnim;

    // Start is called before the first frame update
    void Start()
    {
        doorAnim = this.transform.parent.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        doorAnim.SetBool("isOpening", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Enemy") || other.tag.Equals("Player"))
            doorAnim.SetBool("isOpening", false);
    }
    public void Interact()
    {
       
        doorAnim.SetBool("isOpening", true);
    }
    public void Open()
    {
        doorAnim.SetBool("isOpening", true);
    }
}
