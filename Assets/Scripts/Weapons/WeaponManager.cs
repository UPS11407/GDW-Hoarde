using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public List<Gun> guns;
    public int activeGun;
    public List<int> gunInventory;

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

    public void SwapWeapon(InputAction.CallbackContext ctx)
    {
        if (GameObject.Find("Inventory").transform.GetChild(5).gameObject.activeSelf)
        {
            return;
        }

        if (guns[activeGun].canReload && guns[activeGun].canSwap)
        {
            var scrollFloat = ctx.action.ReadValue<float>();

            int prevGun = activeGun;

            if (activeGun == 3)
            {
                guns[3].ToggleRailgun(false);
            }

            if (scrollFloat > 0)
            {
                if (activeGun + 1 == 3 && gunInventory.Count == 4) guns[3].ToggleRailgun(true);
                activeGun++;
            }
            else
            {
                activeGun--;
            }

            if (activeGun < 0)
            {
                activeGun = gunInventory.Count - 1;

                if (gunInventory.Count == 4) guns[3].ToggleRailgun(true);
            }
            else if (activeGun > gunInventory.Count - 1)
            {
                activeGun = 0;
            }

            if (gunInventory.Count > 1)
            {
                DoWeaponSwap(gunInventory[prevGun], gunInventory[activeGun]);
            }


        }
    }

    public void DoWeaponSwap(int weaponFrom, int weaponTo)
    {
        activeGun = weaponTo;
        guns[weaponFrom].ToggleSelf(false);

        guns[weaponTo].ToggleSelf(true);
        guns[weaponTo].UpdateDisplay();
    }

    public void UpdateWeaponStats()
    {
        guns[activeGun].UpdateWeaponStats();
    }
}
