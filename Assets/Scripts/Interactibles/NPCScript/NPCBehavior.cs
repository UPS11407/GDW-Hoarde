using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehavior : MonoBehaviour, IInteractible    
{
    public int state = 0;
    [SerializeField] TextMeshProUGUI text;
    public void Interact()
    {
        switch (state)
        {
            case 0:
                StartCoroutine(Talk("GIMME BATTERY"));
                break;

            case 1:
                StartCoroutine(Talk("*EATS BATTERY*"));
                break;
        }

    }

    IEnumerator Talk(string dialogueText)
    {
        for (int i = 0; i <= dialogueText.Length; i++)
        {
            string a = dialogueText.Substring(0, i);
            
            text.SetText(a);

            yield return new WaitForSeconds(0.075f);
        }
        yield return new WaitForSeconds(2);
        text.SetText("");
    }
    
}
