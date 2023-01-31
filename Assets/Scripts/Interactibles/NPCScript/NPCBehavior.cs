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

            yield return new WaitForSeconds(0.075f);
        }
        if (passable[state])
        {
            ChangeState();
        }
        UpdateHUD();
        yield return new WaitForSeconds(2);
        interactible = true;
        text.SetText("");
    }

    public void ChangeState()
    {
        state++;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        hudText.text = hudHints[state+1];
    }
}
