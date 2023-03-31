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
    GameObject menuText;
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

                player.GetComponent<WeaponManager>().gunInventory.Add(2);
                player.GetComponent<WeaponManager>().DoWeaponSwap(player.GetComponent<WeaponManager>().activeGun, 2);
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

                GameObject pistol = GameObject.Find("Pistol");
                if(pistol != null)
                {
                    pistol.SetActive(false);
                }
                player.GetComponent<WeaponManager>().gunInventory.Add(3);
                player.GetComponent<WeaponManager>().DoWeaponSwap(player.GetComponent<WeaponManager>().activeGun, 3);
            }
            else
            {
                if (!inventory.CheckForEmptySlot())
                {
                    StartCoroutine(ModMenuText("! Inventory Full !", Color.red));
                    return;
                }

                animator.Play("Cube");
                animator.Play("Glass");
                animator.Play("Handle");
                canInteract = false;

                int randRoll = RollItem();

                inventory.AddRandomItem(randRoll);
                inventory.availableAttachments.Remove(inventory.availableAttachments[randRoll]);
                StartCoroutine(ModMenuText("New Item Added! (Press B)", Color.white));
            }
            Debug.Log("YOU GOT LIGMA");

            spawnLocation.SetActive(true);

            Destroy(gameObject, 6);
            
        }
    }

    IEnumerator ModMenuText(string text, Color color)
    {
        menuText.SetActive(true);
        menuText.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        menuText.transform.GetChild(1).GetComponent<TMP_Text>().color = color;
        yield return new WaitForSeconds(5.9f);
        menuText.SetActive(false);
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        menuText = GameObject.Find("ChestText");
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
