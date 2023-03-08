using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryAttachment;

public class Inventory : MonoBehaviour
{
    public const int MaxInventorySlotsCount = 7;

    public List<InventorySlot> inventorySlots;
    public List<InventorySlot> weaponSlots;
    public List<InventorySlot> ammoSlots;
    public TrashSlot trashSlot;

    public int standardAmmo;
    public int fireAmmo;
    public int slowAmmo;
    public int explosiveAmmo;
    public int railgunCharge;

    public GameObject weaponModCanvas;

    public AmmoType selectedAmmo;

    public void ToggleWeaponModCanvas(bool toggle)
    {
        weaponModCanvas.SetActive(toggle);
    }

    public int UpdateAmmoCount()
    {
        switch (selectedAmmo)
        {
            case AmmoType.STANDARD: return standardAmmo;
            case AmmoType.EXPLOSIVE: return explosiveAmmo;
            case AmmoType.SLOW: return slowAmmo;
            case AmmoType.INCINDIARY: return fireAmmo;
            case AmmoType.RAILGUN: return railgunCharge;
        }

        return 0;
    }

    public bool AddRandomItem()
    {
        if (inventorySlots.Count >= MaxInventorySlotsCount)
        {
            return false;
        }

        foreach (InventorySlot slot in inventorySlots)
        {

        }
        

        return true;
    }

    public void TrashItem()
    {

    }
}
