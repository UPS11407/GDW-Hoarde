using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryAttachment;

public class Inventory : MonoBehaviour
{
    public WeaponManager weaponManager;

    public const int MaxInventorySlotsCount = 8;

    public List<InventorySlotGroups> weaponSlots;
    public List<InventorySlot> inventorySlots;
    public List<InventorySlot> ammoSlots;
    public List<InventoryAttachment> availableAttachments;
    public List<kvp> keys;
    public Dictionary<InventoryAttachment, ScriptableObject> attachmentPairs = new Dictionary<InventoryAttachment, ScriptableObject>();
    public List<ScriptableObject> standardAttachments;

    InventorySlot barrelSlot;
    InventorySlot gripSlot;
    InventorySlot magazineSlot;

    public InventoryItem selectedAmmoSlot;
    public TrashSlot trashSlot;

    public int standardAmmo;
    public int fireAmmo;
    public int slowAmmo;
    public int explosiveAmmo;
    public int railgunCharge;

    public GameObject weaponModCanvas;

    public AmmoType selectedAmmo;

    private void Awake()
    {
        foreach (kvp key in keys)
        {
            attachmentPairs.Add
                (key.attachment, key.attachmentPair);
        }

        weaponModCanvas.SetActive(false);
    }

    public void ToggleWeaponModCanvas(bool toggle)
    {
        weaponModCanvas.SetActive(toggle);
    }

    public void UpdateWeaponType()
    {
        switch (weaponManager.guns[weaponManager.activeGun].gameObject.name)
        {
            case "Pistol":
                RunWeaponLoop(GunType.PISTOL);
                break;

            case "Rifle":
                RunWeaponLoop(GunType.RIFLE);
                break;

            case "Railgun":
                RunWeaponLoop(GunType.RAILGUN);
                break;
        }
    }

    void RunWeaponLoop(GunType gunType)
    {
        foreach (InventorySlot slot in weaponSlots[weaponManager.activeGun].slots)
        {
            slot.slotGunType = gunType;
        }
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

    public void ToggleVisibleSlots(bool toggle)
    {
        if (toggle)
        {
            foreach (InventorySlotGroups weaponSlotGroups in weaponSlots)
            {
                foreach (InventorySlot slot in weaponSlotGroups.slots)
                {
                    if (slot.slotGunType == GunType.PISTOL && weaponManager.GetActiveGun() == 0)
                    {
                        slot.transform.parent.gameObject.SetActive(true);
                    }
                    else if (slot.slotGunType == GunType.RIFLE && weaponManager.GetActiveGun() == 1)
                    {
                        slot.transform.parent.gameObject.SetActive(true);
                    }
                    else if (slot.slotGunType == GunType.RAILGUN && weaponManager.GetActiveGun() == 2)
                    {
                        slot.transform.parent.gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            foreach (InventorySlotGroups weaponSlotGroups in weaponSlots)
            {
                foreach (InventorySlot slot in weaponSlotGroups.slots)
                {
                    slot.transform.parent.gameObject.SetActive(false);
                }
            }
        }
        
    }

    public void TrashItem()
    {

    }

    public void UpdateWeaponStats()
    {
        foreach(InventorySlot slot in weaponSlots[weaponManager.activeGun].slots)
        {
            switch (slot.slotAttachmentType)
            {
                case AttachmentType.BARREL:
                    barrelSlot = slot;
                    break;

                case AttachmentType.MAGAZINE:
                    magazineSlot = slot;
                    break;

                case AttachmentType.GRIP:
                    gripSlot = slot;
                    break;
            }
        }

        weaponManager.guns[weaponManager.activeGun].changeAmmo((WeaponModScriptableObject)attachmentPairs[selectedAmmoSlot.attachment]);

        if (barrelSlot.item != null) weaponManager.guns[weaponManager.activeGun].changeBarrel((WeaponModScriptableObject)attachmentPairs[barrelSlot.item.attachment]);
        else weaponManager.guns[weaponManager.activeGun].changeBarrel((WeaponModScriptableObject)standardAttachments[0]);

        if (gripSlot.item != null) weaponManager.guns[weaponManager.activeGun].changeGrip((WeaponModScriptableObject)attachmentPairs[gripSlot.item.attachment]);
        else weaponManager.guns[weaponManager.activeGun].changeGrip((WeaponModScriptableObject)standardAttachments[1]);

        if (magazineSlot.item != null) weaponManager.guns[weaponManager.activeGun].changeMag((WeaponModScriptableObject)attachmentPairs[magazineSlot.item.attachment]);
        else weaponManager.guns[weaponManager.activeGun].changeMag((WeaponModScriptableObject)standardAttachments[2]);
    }

    public void UpdateWeaponStatsRailgun()
    {
        foreach (InventorySlot slot in weaponSlots[weaponManager.activeGun].slots)
        {
            switch (slot.slotAttachmentType)
            {
                case AttachmentType.BARREL:
                    barrelSlot = slot;
                    break;
            }
        }

        if (barrelSlot != null) weaponManager.guns[weaponManager.activeGun].changeBarrel((WeaponModScriptableObject)attachmentPairs[barrelSlot.item.attachment]);
        else weaponManager.guns[weaponManager.activeGun].changeBarrel((WeaponModScriptableObject)standardAttachments[3]);
    }
}
[System.Serializable]
public class InventorySlotGroups
{
    public List<InventorySlot> slots;
}

[System.Serializable]
public class kvp
{
    public InventoryAttachment attachment;
    public ScriptableObject attachmentPair;
}
