using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour, IInteractible
{
    [SerializeField] NPCBehavior NPC;
    public Generator gen;

    public void Interact()
    {
        NPC.UpdateHUD();
        gen.fuse = true;

        Destroy(gameObject);
    }
}
