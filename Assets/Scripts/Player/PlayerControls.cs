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
    public PlayerInput playerContr;
    

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


        //fire.performed += Fire;         <-- do firing method here

        //heal.performed += Heal;         <-- do healing method here
        //swapMod.performed += SwapMod;   <-- do swap mod method here
        //reload.performed += Reload;     <-- do reload method here

        //interact.performed += ctx => InteractWithObject();

        move.Enable();
        fire.Enable();
        interact.Enable();
        look.Enable();
        reload.Enable();
        swapMod.Enable();
        heal.Enable();
        sprint.Enable();

    }
    private void OnDisable()
    {
        move.Disable();
        interact.Disable();
    }

    
}
