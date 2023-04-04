using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour, IInteractible
{
    [SerializeField] NPCBehavior NPC;
    public Generator gen;

    public void Interact()
    {
        NPC.hintState = 4;
        NPC.UpdateHUD();
        gen.fuse = true;

        Destroy(gameObject);
    }
}
