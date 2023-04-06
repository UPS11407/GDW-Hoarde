using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Generator : MonoBehaviour, IInteractible
{
    public bool fuse = false;
    GameObject[] powerDoors;
    public NPCBehavior johnatelo;
    public GameObject fuseObj;
    public GameObject[] lights;
    public GameObject spawner;
    public GameObject hintText;
    AudioSource generatorSound;
    Animator animator;
    bool canInteract = true;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spawner.SetActive(false);
        generatorSound = GetComponent<AudioSource>();
        powerDoors = GameObject.FindGameObjectsWithTag("Power Door");

        foreach (GameObject door in powerDoors)
        {
            door.GetComponentInChildren<DoorController>().ForceLock();
        }

        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    public void Interact()
    {
        if (canInteract && fuse)
        {
            canInteract = false;
            TurnOnPower();
            animator.Play("TurnOn");
            fuse = false;
            GameObject.Find("HintText").GetComponent<TMP_Text>().text = "Explore the rest of the facility";
        }
    }

    public void TurnOnPower()
    {
        fuseObj.SetActive(true);
        spawner.SetActive(true);
        foreach (GameObject door in powerDoors)
        {
            door.SetActive(false);
        }

        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }

        //generatorSound.Play();
        johnatelo.UpdateHUD();
        StartCoroutine(disableTextBox());
    }

    IEnumerator disableTextBox()
    {
        yield return new WaitForSeconds(60);
        hintText.SetActive(false);
    }
}
