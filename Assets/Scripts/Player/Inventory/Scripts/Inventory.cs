using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum AmmoTypes { STANDARD, EXPLOSIVE, INCINDIARY, SLOW, RAILGUN };

    public const int MaxInventorySlotsCount = 7;

    public List<InventorySlot> inventorySlots;
    public TrashSlot trashSlot;

    public List<InventoryAttachment> allItems;
    public List<InventoryItem> inventoryItems;

    public int standardAmmo;
    public int fireAmmo;
    public int slowAmmo;
    public int explosiveAmmo;
    public int railgunCharge;

    public AmmoTypes selectedAmmo;

    private void Start()
    {
        inventoryItems.Capacity = MaxInventorySlotsCount;
    }

    public int UpdateAmmoCount()
    {
        switch (selectedAmmo)
        {
            case AmmoTypes.STANDARD: return standardAmmo;
            case AmmoTypes.EXPLOSIVE: return explosiveAmmo;
            case AmmoTypes.SLOW: return slowAmmo;
            case AmmoTypes.INCINDIARY: return fireAmmo;
            case AmmoTypes.RAILGUN: return railgunCharge;
        }

        return 0;
    }

    public bool AddItem()
    {



        return true;
    }

    public void TrashItem()
    {

    }
}
