using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public Dictionary<Text, Button> buttons;
    PlayerInput playerContr;

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);

        pauseMenu.OpenMenu();
    }

    public void StartRebind(string inputAction)
    {
        InputActionReference input = ScriptableObject.CreateInstance<InputActionReference>();
        input.Set(playerContr.FindAction(inputAction));



        input.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => EndRebind());
    }

    void EndRebind()
    {

    }
}
