using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour, IInteractible
{
    [SerializeField] NPCBehavior NPC;

    public void Interact()
    {
        NPC.ChangeState();
    }
}
