using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static InventoryAttachment;

public class AmmoBox : MonoBehaviour, IInteractible
{
    GameObject player;
    AmmoType ammo;
    public GameObject spawnLocation;
    bool interactible = true;
    public Material standard;
    public Material incindiary;
    public Material slow;
    public Material explosive;
    GameObject inventoryText;

    private void Awake()
    {
        inventoryText = GameObject.Find("InventoryText");
    }

    void Start()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        player = GameObject.Find("Player");
        int ammoType = Random.Range(0, 101);
        //standard 0-50
        //incin 51-70
        //slow 71-90
        //explosive 91-100

        if(ammoType <= 50)
        {
            ammo = AmmoType.STANDARD;
            gameObject.name = "Standard Ammo Crate";
            foreach(MeshRenderer mesh in meshRenderers)
            {
                mesh.material = standard;
            }
        }else if(ammoType <= 70)
        {
            ammo = AmmoType.INCINDIARY;
            gameObject.name = "Incindiary Ammo Crate";
            foreach (MeshRenderer mesh in meshRenderers)
            {
                mesh.material = incindiary;
            }
        }
        else if(ammoType <= 90)
        {
            ammo = AmmoType.SLOW;
            gameObject.name = "Slow Ammo Crate";
            foreach (MeshRenderer mesh in meshRenderers)
            {
                mesh.material = slow;
            }
        }
        else if(ammoType <= 100)
        {
            ammo = AmmoType.EXPLOSIVE;
            gameObject.name = "Explosive Ammo Crate";
            foreach (MeshRenderer mesh in meshRenderers)
            {
                mesh.material = explosive;
            }
        }
    }

    public void Interact()
    {
        if (interactible)
        {
            var inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

            int r_roll = Random.Range(30, 50);

            inventory.AddAmmo(r_roll, ammo);
            inventory.weaponManager.guns[inventory.weaponManager.activeGun].UpdateDisplay();

            switch (ammo)
            {
                case AmmoType.EXPLOSIVE:
                    StartCoroutine(InventoryText($"{r_roll} explosive ammo added", Color.white));
                    break;

                case AmmoType.STANDARD:
                    StartCoroutine(InventoryText($"{r_roll} standard ammo added", Color.white));
                    break;

                case AmmoType.INCINDIARY:
                    StartCoroutine(InventoryText($"{r_roll} incidiary ammo added", Color.white));
                    break;

                case AmmoType.SLOW:
                    StartCoroutine(InventoryText($"{r_roll} slow ammo added", Color.white));
                    break;
            }

            interactible = false;
            Destroy(gameObject, 5f);
            
        }
    }

    IEnumerator InventoryText(string text, Color color)
    {
        inventoryText.transform.GetChild(0).gameObject.SetActive(true);
        inventoryText.transform.GetChild(1).gameObject.SetActive(true);

        inventoryText.SetActive(true);
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().color = color;

        yield return new WaitForSeconds(4.9f);

        inventoryText.transform.GetChild(0).gameObject.SetActive(false);
        inventoryText.transform.GetChild(1).gameObject.SetActive(false);
    }
}
