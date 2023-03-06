using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehavior : MonoBehaviour, IInteractible    
{
    public int state = 0;
    [SerializeField] TextMeshProUGUI text;
    public string[] dialogue;
    public bool[] passable;
    public TextMeshProUGUI hudText;
    public string[] hudHints;
    bool interactible = true;
    public DoorController genDoor;
    public DoorController[] doorColliders;
    public DoorController[] eventDoors;
    public GameObject[] enemies;
    GameObject player;
    [SerializeField] float dialogueRange;
    private void Start()
    {
        player = GameObject.Find("Player");
        UpdateHUD();
        genDoor.ForceLock();
        foreach(DoorController door in doorColliders)
        {
            door.ForceLock();

        }
        
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }
    private void Update()
    {
        if (interactible == false && Vector3.Distance(transform.position, player.transform.position) > dialogueRange)
        {
            EndDialogue();
        }
    }
    public void Interact()
    {
        if (interactible) StartCoroutine(Talk(dialogue[state]));
    }

    IEnumerator Talk(string dialogueText)
    {
        interactible = false;
        for (int i = 0; i <= dialogueText.Length; i++)
        {
            string a = dialogueText.Substring(0, i);
            
            text.SetText(a);

            yield return new WaitForSeconds(0.0125f);
        }
        if (passable[state])
        {
            ChangeState();
        }
        if (Vector3.Distance(transform.position, player.transform.position) > dialogueRange)
        {
            Debug.Log("BREAK");
            EndDialogue();
            yield break;
        }
        UpdateHUD();
        yield return new WaitForSeconds(5);
        EndDialogue();
    }

    public void ChangeState()
    {
        state++;
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        hudText.text = hudHints[state+1];
        if (state >= 1)
        {
            foreach (DoorController door in doorColliders)
            {
                door.Unlock();
            }


            foreach (GameObject enemy in enemies)
            {
                if(enemy != null) enemy.SetActive(true);
            }
        }

        if (state >= 3)
        {
            genDoor.Unlock();
        }
    }

    public void EndDialogue()
    {
        interactible = true;
        text.SetText("");
    }
}
