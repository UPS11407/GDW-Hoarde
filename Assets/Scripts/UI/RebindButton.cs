using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindButton : MonoBehaviour
{
    public GameObject player;
    public PlayerInput playerInput;
    public InputActionReference input;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebind()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = "Waiting for input...";

        rebindingOperation = input.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => EndRebind())
            .Start();
    }

    void EndRebind()
    {
        var control = rebindingOperation.selectedControl;

        rebindingOperation.Dispose();

        gameObject.GetComponentInChildren<TMP_Text>().text = control.ToString().Split('/')[2];

        player.GetComponent<PlayerControlsManager>().ReinitPlayerActions();
    }
}
