using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard4 : MonoBehaviour, IInteractible
{

    public GameObject[] enemies;
    GameObject rafielo;
    Inventory inventory;
    public int level;

    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        rafielo = GameObject.Find("Rafielo OMallyOConnelSullivan");
    }
    public void Interact()
    {

        rafielo.GetComponent<Rafielo>().state = 1;

        rafielo.GetComponent<Rafielo>().dialogueState = 0;
        rafielo.GetComponent<Rafielo>().UpdateHUD();
        inventory.SetKeycardLevel(level);
        foreach (GameObject enemy in enemies)
        {

            if (enemy != null) enemy.SetActive(true);

        }
        Destroy(this.gameObject);
    }
}