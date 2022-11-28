using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarrelUpgrades
{
    public bool standard = true;
    public bool shotgun = false;
}

[System.Serializable]
public class GripUpgrades
{
    public bool standard = true;
    public bool fullAuto = false;
}

[System.Serializable]
public class AmmoUpgrades
{
    public bool standard = true;
    public bool explosive = false;
}

[System.Serializable]
public class MagazineUpgrades
{
    public bool standard = true;
    public bool extended = false;
}

public class WeaponInventory : MonoBehaviour
{
    public BarrelUpgrades barrelUpgrades;
    public GripUpgrades gripUpgrades;
    public AmmoUpgrades ammoUpgrades;
    public MagazineUpgrades magazineUpgrades;

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            barrelUpgrades.shotgun = true;
        }
    }*/
}