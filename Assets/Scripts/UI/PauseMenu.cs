using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public WeaponManager weaponManager;
    PlayerInput playerContr;
    PlayerControlsManager playerControlsManager;

    ControlsMenu controlsMenu;

    private void Awake()
    {
        playerContr = player.GetComponent<PlayerInput>();
        playerControlsManager = player.GetComponent<PlayerControlsManager>();
    }

    public void RunPause()
    {
        if(menu.activeInHierarchy == true)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        playerContr.SwitchCurrentActionMap("Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
