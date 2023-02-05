using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarrelUpgrades
{
    public bool standard = true;
    public bool shotgun = false;
    public bool sniper = false;
}

[System.Serializable]
public class GripUpgrades
{
    public bool standard = true;
    public bool fullAuto = false;
    public bool burst = false;
}

[System.Serializable]
public class AmmoUpgrades
{
    public bool standard = true;
    public bool explosive = false;
    public bool slow = false;
}

[System.Serializable]
public class MagazineUpgrades
{
    public bool standard = true;
    public bool extended = false;
    public bool quick = false;
}

public class WeaponInventory : MonoBehaviour
{
    public BarrelUpgrades barrelUpgrades;
    public GripUpgrades gripUpgrades;
    public AmmoUpgrades ammoUpgrades;
    public MagazineUpgrades magazineUpgrades;
}