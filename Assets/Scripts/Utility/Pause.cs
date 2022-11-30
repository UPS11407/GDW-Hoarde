using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    public GameObject menu;
    public GameObject rifle;
    public GameObject pistol;
    public GameObject player;

    PlayerInput playerInputs;
    InputAction pause;

    private void Awake()
    {
        playerInputs = new PlayerInput();
    }

    private void OnEnable()
    {
        pause = playerInputs.Player.Pause;
        pause.Enable();
        pause.performed += ctx => OpenMenu();
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.SetActive(true);
        Time.timeScale = 0;
        rifle.GetComponent<Gun>().enabled = false;
        pistol.GetComponent<Gun>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        menu.SetActive(false);
        Time.timeScale = 1.0f;
        rifle.GetComponent<Gun>().enabled = true;
        pistol.GetComponent<Gun>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
