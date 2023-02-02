using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IInteractible
{
    public bool fuse = false;
    GameObject[] powerDoors;
    public NPCBehavior johnatelo;
    public GameObject[] lights;
    public GameObject spawner;
    AudioSource generatorSound;
    Animator animator;
    bool canInteract = true;

    private void Awake()
    {
        animator = gameObject.transform.parent.parent.GetComponent<Animator>();
        spawner.SetActive(false);
        generatorSound = GetComponent<AudioSource>();
        powerDoors = GameObject.FindGameObjectsWithTag("Power Door");

        foreach (GameObject door in powerDoors)
        {
            Destroy(door.transform.GetChild(0).gameObject);
        }

        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    public void Interact()
    {
        if (canInteract)
        {
            canInteract = false;
            if (fuse)
            {
                TurnOnPower();
                animator.Play("TurnOn");
                fuse = false;
            }
        }
    }

    public void TurnOnPower()
    {
        spawner.SetActive(true);
        foreach (GameObject door in powerDoors)
        {
            door.GetComponent<Animator>().SetBool("isOpening", true);
        }

        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }

        //generatorSound.Play();

        johnatelo.ChangeState();
    }
}
