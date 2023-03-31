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
    [SerializeField] GameObject DialogueBox;

    public List<Dialogue> dialogue;

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
        DialogueBox.SetActive(true);
        Time.timeScale = 0;
        playerControlsManager.playerInput.SwitchCurrentActionMap("Menu");
        interactible = false;

        int dialogueContinuation = dialogueState == dialogueText.Count - 1 ? dialogueText.Count + 1 : dialogueText.Count;

        for (int i = dialogueState; i < dialogueContinuation - 1; i++)
        {

            NPCName.text = dialogueText[dialogueState].name;

            for (int j = 0; j <= dialogueText[dialogueState].text.Length; j++)
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

            if (dialogueState + 1 != dialogueText.Count)
            {
                dialogueState++;
            }
        }


        EndDialogue();
        UpdateHUD();
    }

    public void ChangeState()
    {
        state++;
        dialogueState = 0;
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
        DialogueBox.SetActive(false);
        Time.timeScale = 1;
        text.SetText("");
        NPCName.SetText("");
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
