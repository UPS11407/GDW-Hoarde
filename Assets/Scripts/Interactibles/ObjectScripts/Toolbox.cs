using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour, IInteractible
{
    public bool obtainable = false;
    public void Interact()
    {
        if (obtainable)
        {
            GameObject.Find("Mikael Angeloskovitch").GetComponent<NPCBehavior>().state = 3;
            GameObject.Find("Mikael Angeloskovitch").GetComponent<NPCBehavior>().UpdateHUD();
        }
    }
}
