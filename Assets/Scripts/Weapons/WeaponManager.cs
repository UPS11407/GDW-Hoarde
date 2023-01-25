using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Gun> guns;
    public int activeGun;
    public int[] gunInventory = new int[2];

    public int GetActiveGun()
    {
        return activeGun;
    }

    public void SetActiveGun(int gun)
    {
        activeGun = gun;
    }

    public void Reload()
    {
        guns[activeGun].RunReload();
    }

    public void ToggleFire(bool fire)
    {
        guns[activeGun].ToggleFire(fire);
    }

    public void ToggleFireButton(bool toggle)
    {
        guns[activeGun].ToggleFireButton(toggle);
    }

    public void SwapWeapon()
    {
        if (guns[activeGun].canReload && guns[activeGun].canSwap)
        {
            if (activeGun == gunInventory[0])
            {
                activeGun = gunInventory[1];
                DoWeaponSwap(gunInventory[0], gunInventory[1]);
            }
            else
            {
                activeGun = gunInventory[0];
                DoWeaponSwap(gunInventory[1], gunInventory[0]);
            }
        }
    }

    public void DoWeaponSwap(int weaponFrom, int weaponTo)
    {
        guns[weaponFrom].ToggleSelf(false);

        guns[weaponTo].ToggleSelf(true);
        guns[weaponTo].UpdateDisplay();
    }

    public void UpdateWeaponStats()
    {
        guns[activeGun].UpdateWeaponStats();
    }

    public void ToggleWeaponModCanvas(bool toggle)
    {
        guns[activeGun].ToggleWeaponModCanvas(toggle);
    }
}
