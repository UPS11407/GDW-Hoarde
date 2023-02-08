using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindButton : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public PlayerControlsManager player;
    public bool isComposite;
    public string input;

    private void Start()
    {
        DoButtonText();
    }
    
    public void StartRebind()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = ">  <";

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
        int bindingIndex = rebindingOperation.action.GetBindingIndexForControl(rebindingOperation.selectedControl);

        gameObject.GetComponentInChildren<TMP_Text>().text =
        InputControlPath.ToHumanReadableString(
            rebindingOperation.action.bindings[bindingIndex].effectivePath, 
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        player.SaveBindings();
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
