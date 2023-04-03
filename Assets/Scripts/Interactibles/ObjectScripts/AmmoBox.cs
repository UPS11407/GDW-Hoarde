using System.Collections;
using System.Collections.Generic;
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

            inventory.AddAmmo(Random.Range(20, 40), ammo);
            inventory.weaponManager.guns[inventory.weaponManager.activeGun].UpdateDisplay();

            interactible = false;
            Destroy(gameObject, 1.5f);
            
        }
    }
}
