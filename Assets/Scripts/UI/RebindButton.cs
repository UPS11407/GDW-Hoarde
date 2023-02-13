using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindButton : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public PlayerControlsManager playerControlsManager;
    public Player player;
    public bool isComposite;
    public string input;

    Button button;

    private void Start()
    {
        DoButtonText();
        button = GetComponent<Button>();
    }
    
    public void StartRebind()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = ">  <";
        button.interactable = false;

        rebindingOperation = playerInput.actions[input].PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation => EndRebind())
            .OnCancel(operation => CancelRebind())
            .Start();
    }

    public void StartRebindComposite()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = ">  <";
        button.interactable = false;

        var bindingIndex = playerInput.actions["Move"].bindings.IndexOf(x => x.isPartOfComposite && x.name == input);

        rebindingOperation = playerInput.actions["Move"].PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithTargetBinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation => EndRebind())
            .OnCancel(operation => CancelRebind())
            .Start();
    }

    void CancelRebind()
    {
        button.interactable = true;

        if (!isComposite)
        {
            gameObject.GetComponentInChildren<TMP_Text>().text =
                InputControlPath.ToHumanReadableString(
                playerInput.actions[input].bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        else
        {
            gameObject.GetComponentInChildren<TMP_Text>().text =
                InputControlPath.ToHumanReadableString(
                playerInput.actions["Move"].bindings[playerInput.actions["Move"].bindings.IndexOf(x => x.isPartOfComposite && x.name == input)].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        rebindingOperation.Dispose();
    }

    void EndRebind()
    {
        button.interactable = true;

        int bindingIndex = rebindingOperation.action.GetBindingIndexForControl(rebindingOperation.selectedControl);

        gameObject.GetComponentInChildren<TMP_Text>().text =
        InputControlPath.ToHumanReadableString(
            rebindingOperation.action.bindings[bindingIndex].effectivePath, 
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        playerControlsManager.SaveBindings();

        player.UpdateBindings();
    }

    public void DoButtonText()
    {
        if (!isComposite)
        {
            gameObject.GetComponentInChildren<TMP_Text>().text =
                InputControlPath.ToHumanReadableString(
                playerInput.actions[input].bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        else
        {
            gameObject.GetComponentInChildren<TMP_Text>().text =
                InputControlPath.ToHumanReadableString(
                playerInput.actions["Move"].bindings[playerInput.actions["Move"].bindings.IndexOf(x => x.isPartOfComposite && x.name == input)].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
}
