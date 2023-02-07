using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public GameObject HUD;
    public WeaponManager weaponManager;
    PlayerInput playerContr;

    ControlsMenu controlsMenu;

    public void RunPause()
    {
        if (playerContr != null)
        {
            if (menu.activeInHierarchy == true)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
        else
        {
            playerContr = player.GetComponent<PlayerInput>();
            RunPause();
        }

        Debug.Log(playerContr.currentActionMap.ToString());
    }

    public void OpenMenu()
    {
        playerContr.SwitchCurrentActionMap("Menu");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        HUD.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 0;
        foreach(Gun guns in weaponManager.guns)
        {
            guns.canSwap = false;
            guns.enabled = false;
        }
    }

    public void CloseMenu()
    {
        playerContr.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        HUD.SetActive(true);
        menu.SetActive(false);
        Time.timeScale = 1.0f;

        foreach (Gun guns in weaponManager.guns)
        {
            guns.canSwap = true;
            guns.enabled = true;
        }
    }

    public void OpenControlsMenu()
    {
        CloseMenu();

        controlsMenu.OpenMenu();
    }
}
