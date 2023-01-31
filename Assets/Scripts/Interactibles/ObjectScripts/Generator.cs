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

    private void Awake()
    {
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
        if (fuse)
        {
            TurnOnPower();
            fuse = false;
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

        generatorSound.Play();

        johnatelo.ChangeState();
    }
}
