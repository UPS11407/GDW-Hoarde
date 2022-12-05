using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Barrels
{
    public GameObject topLeft;
    public GameObject bottomLeft;
}

[System.Serializable]
public class Grips
{
    public GameObject topLeft;
    public GameObject bottomLeft;
}

[System.Serializable]
public class Ammo
{
    public GameObject topLeft;
    public GameObject bottomLeft;
}

[System.Serializable]
public class Magazines
{
    public GameObject topLeft;
    public GameObject bottomLeft;
}

public class WeaponModUI : MonoBehaviour
{
    public WeaponInventory inventory;

    public Barrels barrels;
    public Grips grips;
    public Ammo ammo;
    public Magazines magazines;
    public GameObject menuText;

    private void OnEnable()
    {
        menuText.SetActive(false);
        GrandSetup();
    }

    //brainrot
    void GrandSetup()
    {
        if (!inventory.barrelUpgrades.shotgun)
        {
            barrels.bottomLeft.GetComponent<Button>().interactable = false;
        }
        else
        {
            barrels.bottomLeft.GetComponent<Button>().interactable = true;
        }

        if (!inventory.gripUpgrades.fullAuto)
        {
            grips.bottomLeft.GetComponent<Button>().interactable = false;
        }
        else
        {
            grips.bottomLeft.GetComponent<Button>().interactable = true;
        }

        if (!inventory.ammoUpgrades.explosive)
        {
            ammo.bottomLeft.GetComponent<Button>().interactable = false;
        }
        else
        {
            ammo.bottomLeft.GetComponent<Button>().interactable = true;
        }

        if (!inventory.magazineUpgrades.extended)
        {
            magazines.bottomLeft.GetComponent<Button>().interactable = false;
        }
        else
        {
            magazines.bottomLeft.GetComponent<Button>().interactable = true;
        }
    }
}
