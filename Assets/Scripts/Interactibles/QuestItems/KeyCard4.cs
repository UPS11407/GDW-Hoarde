using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard4 : MonoBehaviour, IInteractible
{

    public GameObject[] enemies;
    GameObject rafielo;
    private void Start()
    {
        rafielo = GameObject.Find("Rafielo O’Mally’O’Connel’Sullivan");
    }
    public void Interact()
    {

        rafielo.GetComponent<Rafielo>().state = 1;

        rafielo.GetComponent<Rafielo>().dialogueState = 0;
        rafielo.GetComponent<Rafielo>().UpdateHUD();

        foreach (GameObject enemy in enemies)
        {

            if (enemy != null) enemy.SetActive(true);

        }
        Destroy(this.gameObject);
    }
}