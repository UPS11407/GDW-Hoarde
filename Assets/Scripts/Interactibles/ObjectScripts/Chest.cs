using System.Collections;
using System.Collections.Generic;
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
    public GameObject menuText;
    Inventory inventory;

    public bool tutorialChest;
    public bool railChest;

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

                GameObject.Find("Pistol").GetComponent<Gun>().canSwap = true;
                player.GetComponent<WeaponManager>().SwapWeapon();
                GameObject.Find("Johnatelo").GetComponent<NPCBehavior>().state = 2;
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
                player.GetComponent<WeaponManager>().gunInventory = new int[]{1, 2};
                player.GetComponent<WeaponManager>().SetActiveGun(2);
            }
            else
            {
                if (!inventory.CheckForEmptySlot())
                {
                    Debug.Log("Inventory Full *UI PLACEHOLDER*");
                    return;
                }

                animator.Play("Cube");
                animator.Play("Glass");
                animator.Play("Handle");
                canInteract = false;

                inventory.AddRandomItem(RollItem());
                StartCoroutine(ModMenuText());
            }
            Debug.Log("YOU GOT LIGMA");
            
            Destroy(gameObject, 6);
            
        }
    }

    IEnumerator ModMenuText()
    {
        menuText.SetActive(true);
        yield return new WaitForSeconds(5.9f);
        menuText.SetActive(false);
    }

    private void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        //menuText = GameObject.Find("ChestText");
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

    int RollItem()
    {
        return Random.Range(0, inventory.availableAttachments.Count);
    }
}
