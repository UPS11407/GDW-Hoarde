using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwap : MonoBehaviour
{
    [SerializeField] GameObject weapon1;
    [SerializeField] GameObject weapon2;

    InputAction weaponSwap;
    public PlayerInput playerContr;
    private void Awake()
    {
        playerContr = new PlayerInput();
    }

    private void OnEnable()
    {
        weaponSwap = playerContr.Player.SwapWeapon;
        weaponSwap.Enable();
        weaponSwap.performed += ctx => SwapWeapon();
    }
    public void SwapWeapon()
    {
        if(weapon1.active == false)
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
        } else
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
        }
        
    }
}
