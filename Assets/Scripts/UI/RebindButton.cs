using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindButton : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebind(string input)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = "Waiting for input...";

        rebindingOperation = playerInput.actions[input].PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => EndRebind())
            .Start();
    }

    public void StartRebindComposite(string input)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = "Waiting for input...";

        var bindingIndex = playerInput.actions["Move"].bindings.IndexOf(x => x.isPartOfComposite && x.name == input);

        rebindingOperation = playerInput.actions["Move"].PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .WithTargetBinding(bindingIndex)
            .OnComplete(operation => EndRebind())
            .Start();
    }

    void EndRebind()
    {
        var control = rebindingOperation.selectedControl;

        rebindingOperation.Dispose();

        gameObject.GetComponentInChildren<TMP_Text>().text = control.ToString().Split('/')[2];
    }
}
