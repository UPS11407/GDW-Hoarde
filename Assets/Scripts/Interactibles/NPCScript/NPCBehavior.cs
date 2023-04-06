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
    bool interactible = true;
    protected GameObject player;
    public float dialogueRange;
    public bool lookToPlayer;

    public Vector3 initialDir;
    public float stareDistance;

    private void Awake()
    {
        initialDir = transform.localRotation.eulerAngles;
    }

    protected void Startup()
    {
        player = GameObject.Find("Player");
        UpdateHUD();
        
    }

    private void Update()
    {
        Vector3 dir2P = (new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z)).normalized;
      
        Quaternion target = Quaternion.LookRotation(dir2P);

        if (lookToPlayer)
        {
            Debug.Log(gameObject.transform.name);
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= stareDistance)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, target, 0.05f);
            }
            //transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, transform.rotation.y, 0), dir2P, 0.1f));
            else transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(initialDir), 0.025f);

        }
    }
    public void Interact()
    {
        if (interactible) StartCoroutine(Talk(dialogue[state].dialogues));
    }

    IEnumerator Talk(List<DialogueText> dialogueText)
    {
        playerControlsManager.talkingTo = this;

        if (dialogueState + 1 != dialogueText.Count)
        {
            dialogueState = 0;
        }

        Time.timeScale = 0;
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
                UpdateHUD();
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
        hudText.text = dialogue[state].dialogues[dialogueState].hint;
    }

    public void EndDialogue()
    {
        Time.timeScale = 1.0f;
        interactible = true;
        DialogueBox.SetActive(false);
        Time.timeScale = 1;
        text.SetText("");
        NPCName.SetText("");
        playerControlsManager.playerInput.SwitchCurrentActionMap("Player");
        StopAllCoroutines();

        playerControlsManager.talkingTo = null;
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
        public string hint;
    }
}
