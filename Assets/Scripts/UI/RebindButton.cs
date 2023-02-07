using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindButton : MonoBehaviour
{
    GameObject player;
    public InputActionReference input;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

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


    }
}
