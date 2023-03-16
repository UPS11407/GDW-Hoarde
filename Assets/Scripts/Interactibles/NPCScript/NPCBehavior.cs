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
    protected GameObject player;
    public float dialogueRange;

    protected void Startup()
    {
        player = GameObject.Find("Player");
        UpdateHUD();
    }

    private void Update()
    {
        if (interactible == false && Vector3.Distance(transform.position, player.transform.position) > dialogueRange)
        {
            EndDialogue();
        }

        transform.forward =  new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
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

    public virtual void UpdateHUD()
    {
        hudText.gameObject.SetActive(true);
        hudText.transform.parent.gameObject.SetActive(true);
        hudText.text = hudHints[state + 1];
    }

    public void EndDialogue()
    {
        interactible = true;
        text.SetText("");
    }
}
