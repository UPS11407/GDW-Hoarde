using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindButton : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public PlayerControlsManager player;
    public string input;

    private void Start()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text =
        InputControlPath.ToHumanReadableString(
            playerInput.actions[input].bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void StartRebind()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = "Waiting for input...";

        rebindingOperation = playerInput.actions[input].PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation => EndRebind())
            .OnCancel(operation => CancelRebind())
            .Start();
    }

    public void StartRebindComposite()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = "Waiting for input...";

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
        gameObject.GetComponentInChildren<TMP_Text>().text =
        InputControlPath.ToHumanReadableString(
            playerInput.actions[input].bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

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
}
