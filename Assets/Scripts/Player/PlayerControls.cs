using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
public class PlayerControls : MonoBehaviour
{
    InputAction move;
    InputAction fire;
    InputAction interact;
    InputAction look;
    InputAction reload;
    InputAction swapMod;
    InputAction heal;
    InputAction sprint;
    InputAction crouch;

    public PlayerMovement playerMovement { get; }
    public PlayerInput playerContr { get; }
    public Player player { get; }


    private void OnEnable()
    {
        move = playerContr.Player.Move;
        fire = playerContr.Player.Fire;
        interact = playerContr.Player.Interact;
        look = playerContr.Player.Look;
        reload = playerContr.Player.Reload;
        swapMod = playerContr.Player.SwapMod;
        heal = playerContr.Player.Heal;
        sprint = playerContr.Player.Sprint;
        crouch = playerContr.Player.Crouch;

        interact.performed += ctx => player.InteractWithObject();
        heal.performed += ctx => player.HealHP(player.maxHP * 0.3f, true);
        sprint.performed += playerMovement.Sprint;

        move.Enable();
        fire.Enable();
        interact.Enable();
        look.Enable();
        reload.Enable();
        swapMod.Enable();
        heal.Enable();
        sprint.Enable();
        crouch.Enable();

    }
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        interact.Disable();
        look.Disable();
        reload.Disable();
        swapMod.Disable();
        heal.Disable();
        sprint.Disable();
        crouch.Disable();
    }

    
}
