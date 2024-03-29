using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject dialogueMenu;
    public GameObject menu;
    public GameObject player;
    public GameObject HUD;
    public ControlsMenu controlsMenu;
    public PlayerControlsManager playerControlsManager;
    public GameObject mainCamera;

    PlayerInput playerContr;

    public void RunPause()
    {
        if (dialogueMenu.activeSelf)
        {
            playerControlsManager.talkingTo.EndDialogue();
            return;
        }

        if (mainCamera.transform.parent.name != "Camera Parent")
        {
            mainCamera.transform.parent.GetComponent<Computer>().ExitMenu();
            return;
        }

        if (!playerControlsManager.inventory.weaponModCanvas.transform.GetChild(5).gameObject.activeSelf)
        {
            if (playerContr != null)
            {
                if (menu.activeInHierarchy == true)
                {
                    CloseMenu();
                }
                else
                {
                    if (controlsMenu.gameObject.activeInHierarchy)
                    {
                        controlsMenu.CloseMenu();
                    }
                    else
                    {
                        OpenMenu();
                    }

                }
            }
            else
            {
                playerContr = player.GetComponent<PlayerInput>();
                RunPause();
            }
        }
        else
        {
            playerControlsManager.CloseMenu();
        }
    }

    public void OpenMenu()
    {
        playerContr.SwitchCurrentActionMap("Menu");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        HUD.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        playerContr.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        HUD.SetActive(true);
        menu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OpenControlsMenu()
    {
        menu.SetActive(false);
        controlsMenu.OpenMenu();
    }
}
