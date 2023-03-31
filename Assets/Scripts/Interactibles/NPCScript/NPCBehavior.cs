using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehavior : MonoBehaviour, IInteractible    
{
    public PlayerControlsManager playerControlsManager;

    public int state = 0;
    public int dialogueState = 0;

    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text NPCName;

    public List<Dialogue> dialogue;

    public bool[] passable;
    public TextMeshProUGUI hudText;
    public string[] hudHints;
    bool interactible = true;
    protected GameObject player;
    public float dialogueRange;
    public bool lookToPlayer;

    protected void Startup()
    {
        player = GameObject.Find("Player");
        UpdateHUD();
    }

    private void Update()
    {
        if (lookToPlayer)
        {
            transform.forward = new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
    public void Interact()
    {
        if (interactible) StartCoroutine(Talk(dialogue[state].dialogues));
    }

    IEnumerator Talk(List<DialogueText> dialogueText)
    {

        playerControlsManager.playerInput.SwitchCurrentActionMap("Menu");
        interactible = false;

        if (dialogueState + 1 > dialogueText.Count)
        {
            dialogueState--;
        }

        for (int i = 0; i < dialogueText.Count; i++)
        {

            for (int j = dialogueState; j <= dialogueText[dialogueState].text.Length; j++)
            {
                string a = dialogueText[dialogueState].text.Substring(0, j);

                text.SetText(a);

                if (playerControlsManager.progressDialogue)
                {
                    text.SetText(dialogueText[dialogueState].text);
                    playerControlsManager.SetProgressDialogue(false);
                    break;
                }

                yield return new WaitForSecondsRealtime(0.0125f);
            }

            yield return new WaitUntil(() => playerControlsManager.progressDialogue);

            playerControlsManager.SetProgressDialogue(false);
            dialogueState++;
        }

        if (passable[state])
        {
            ChangeState();
        }


        EndDialogue();
        UpdateHUD();
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
        playerControlsManager.playerInput.SwitchCurrentActionMap("Player");
    }

    [System.Serializable]
    public class Dialogue
    {
        public List<DialogueText> dialogues;
    }

    [System.Serializable]
    public class DialogueText
    {
        public string name;
        public string text;
    }
}
