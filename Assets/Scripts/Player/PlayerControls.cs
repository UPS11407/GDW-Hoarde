using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    InputAction fire;
    InputAction interact;
    InputAction reload;
    InputAction swapMod;
    InputAction heal;
    InputAction sprint;
    InputAction crouch;
    InputAction swapWeapon;

    PlayerMovement playerMovement;
    PlayerInput playerContr;
    Player player;
    WeaponManager weaponManager;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
        weaponManager = GetComponent<WeaponManager>();
    }

    private void OnEnable()
    {
        fire = playerContr.Player.Fire;
        interact = playerContr.Player.Interact;
        reload = playerContr.Player.Reload;
        swapMod = playerContr.Player.SwapMod;
        heal = playerContr.Player.Heal;
        sprint = playerContr.Player.Sprint;
        crouch = playerContr.Player.Crouch;
        swapWeapon = playerContr.Player.SwapWeapon;

        

        //fire
        interact.performed += ctx => player.InteractWithObject();
        reload.performed += ctx => weaponManager.Reload();
        swapMod.performed += ctx => ToggleMenu();
        heal.performed += ctx => player.HealHP(player.maxHP * 0.3f, true);
        sprint.performed += ctx => playerMovement.Sprint(ctx, true);
        sprint.canceled += ctx => playerMovement.Sprint(ctx, false);
        //crouch
        swapWeapon.performed += ctx => weaponManager.SwapWeapon();

        fire.Enable();
        interact.Enable();
        reload.Enable();
        swapMod.Enable();
        heal.Enable();
        sprint.Enable();
        crouch.Enable();
        swapWeapon.Enable();
    }

    private void OnDisable()
    {
        fire.Disable();
        interact.Disable();
        reload.Disable();
        swapMod.Disable();
        heal.Disable();
        sprint.Disable();
        crouch.Disable();
        swapWeapon.Disable();
    }

    void ToggleMenu()
    {

    }
}
