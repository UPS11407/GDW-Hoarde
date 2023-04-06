using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox : MonoBehaviour, IInteractible
{
    
    public GameObject[] enemies;
    GameObject leo;
    private void Start()
    {
        leo = GameObject.Find("Leo Di Caprisun");
    }
    public void Interact()
    {

        leo.GetComponent<Leo>().state = 1;

        leo.GetComponent<Leo>().dialogueState = 0;
        leo.GetComponent<Leo>().UpdateHUD();


        foreach (GameObject enemy in enemies)
        {

            if (enemy != null) enemy.SetActive(true);

        }
        Destroy(this.gameObject);
    }
}

