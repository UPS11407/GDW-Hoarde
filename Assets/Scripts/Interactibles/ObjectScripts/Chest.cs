using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
I will come back to this later
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Chest))]
public class ChestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        //DrawDefaultInspector();

        Chest script = (Chest)target;

        script.menuText = EditorGUILayout.ObjectField("Menu Text", script.menuText, typeof(GameObject), true) as GameObject;

        script.tutorialChest = EditorGUILayout.Toggle("Tutorial Chest", script.tutorialChest);
        if (script.tutorialChest)
        {
            script.enemyPrefab = EditorGUILayout.ObjectField("Enemy Prefab", script.enemyPrefab, typeof(GameObject), true) as GameObject;
            script.enemyCollection = EditorGUILayout.ObjectField("Enemy Collection", script.enemyCollection, typeof(GameObject), true) as GameObject;
            script.location = EditorGUILayout.ObjectField("Spawn Locations", script.location, typeof(GameObject), true) as GameObject;
        }
    }
}
#endif
*/

public class Chest : MonoBehaviour, IInteractible
{
    int itemIndex;
    GameObject player;
    bool canInteract = true;
    Animator animator;
    GameObject inventoryText;
    Inventory inventory;

    public bool tutorialChest;
    public bool railChest;


    public GameObject spawnLocation;

    //[HideInInspector]
    public GameObject enemyPrefab;

    //[HideInInspector]
    public Transform[] locations;

    //[HideInInspector]
    public GameObject enemyCollection;

    public void Interact()
    {
        if (canInteract)
        {
            
            if (tutorialChest)
            {
                animator.Play("Cube");
                animator.Play("Glass");
                animator.Play("Handle");
                canInteract = false;

                WeaponManager weaponManager = player.GetComponent<WeaponManager>();

                weaponManager.guns[weaponManager.activeGun].canReload = true;
                weaponManager.guns[weaponManager.activeGun].canShoot = true;
                weaponManager.guns[weaponManager.activeGun].canSwap = true;
                weaponManager.guns[weaponManager.activeGun].isReloading = false;

                weaponManager.gunInventory.Add(2);
                weaponManager.DoWeaponSwap(player.GetComponent<WeaponManager>().activeGun, 2);
                GameObject.Find("Johnatelo").GetComponent<NPCBehavior>().state = 1;
                GameObject.Find("Johnatelo").GetComponent<NPCBehavior>().dialogueState = 0;
                GameObject.Find("Johnatelo").GetComponent<NPCBehavior>().UpdateHUD();

                foreach(Transform location in locations)
                {
                    Instantiate(enemyPrefab, location.position, Quaternion.Euler(0, 0, 0), enemyCollection.transform);
                }
                
            }
            else if (railChest)
            {
                animator.Play("Cube");
                animator.Play("Glass");
                animator.Play("Handle");
                canInteract = false;

                WeaponManager weaponManager = player.GetComponent<WeaponManager>();

                weaponManager.guns[weaponManager.activeGun].canReload = true;
                weaponManager.guns[weaponManager.activeGun].canShoot = true;
                weaponManager.guns[weaponManager.activeGun].canSwap = true;
                weaponManager.guns[weaponManager.activeGun].isReloading = false;

                weaponManager.gunInventory.Add(3);
                weaponManager.DoWeaponSwap(player.GetComponent<WeaponManager>().activeGun, 3);
            }
            else
            {
                if (!inventory.CheckForEmptySlot())
                {
                    StartCoroutine(InventoryText("! Inventory Full !", Color.red));
                    return;
                }

                animator.Play("Cube");
                animator.Play("Glass");
                animator.Play("Handle");
                canInteract = false;

                int randRoll = RollItem();

                inventory.AddRandomItem(randRoll);
                inventory.availableAttachments.Remove(inventory.availableAttachments[randRoll]);
                StartCoroutine(InventoryText("New Item Added! (Press B)", Color.white));
            }
            Debug.Log("YOU GOT LIGMA");

            spawnLocation.SetActive(true);

            Destroy(gameObject, 6);
            
        }
    }

    IEnumerator InventoryText(string text, Color color)
    {
        inventoryText.transform.GetChild(0).gameObject.SetActive(true);
        inventoryText.transform.GetChild(1).gameObject.SetActive(true);

        inventoryText.SetActive(true);
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().color = color;

        yield return new WaitForSeconds(5.9f);

        inventoryText.transform.GetChild(0).gameObject.SetActive(false);
        inventoryText.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        inventoryText = GameObject.Find("InventoryText");
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    int RollItem()
    {
        if (player.GetComponent<WeaponManager>().gunInventory.Contains(3))
        {
            return Random.Range(0, inventory.availableAttachments.Count + 1);
        }

        return Random.Range(0, inventory.availableAttachments.Count);
    }
}
