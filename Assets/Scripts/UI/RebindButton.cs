using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindButton : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public PlayerControlsManager player;

    public void StartRebind(string input)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = "Waiting for input...";

        rebindingOperation = playerInput.actions[input].PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation => EndRebind())
            .OnCancel(operation => EndRebind())
            .Start();
    }

    public void StartRebindComposite(string input)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = "Waiting for input...";

        var bindingIndex = playerInput.actions["Move"].bindings.IndexOf(x => x.isPartOfComposite && x.name == input);

        rebindingOperation = playerInput.actions["Move"].PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithTargetBinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation => EndRebind())
            .OnCancel(operation => EndRebind())
            .Start();
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
