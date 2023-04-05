using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InventoryAttachment;

public class Inventory : MonoBehaviour
{
    public List<GameObject> keycards;
    public GameObject inventoryText;
    int keycardLevel;

    public WeaponManager weaponManager;

    public const int MaxInventorySlotsCount = 4;

    public List<InventorySlotGroups> weaponSlots;
    public List<InventorySlot> inventorySlots;
    public List<InventorySlot> ammoSlots;
    public List<InventoryAttachment> availableAttachments;
    public InventoryAttachment railGunAttachment;
    public List<kvp> keys;
    public Dictionary<InventoryAttachment, ScriptableObject> attachmentPairs = new Dictionary<InventoryAttachment, ScriptableObject>();
    public List<WeaponModScriptableObject> standardAttachments;

    InventorySlot barrelSlot;
    InventorySlot gripSlot;
    InventorySlot magazineSlot;

    public InventoryItem selectedAmmoSlot;
    public InventorySlot trashSlot;

    public AmmoType prevAmmo;
    public InventoryAttachment prevMag;

    public int standardAmmo;
    public int fireAmmo;
    public int slowAmmo;
    public int explosiveAmmo;
    public float railgunCharge;

    public float railgunChargeRate;

    public GameObject weaponModCanvas;

    public GameObject itemPrefab;

    public AmmoType selectedAmmo;
    public RailgunModScriptableObject standardRailgunAttachment;

    public Image railgunBar;

    private void Awake()
    {
        foreach (kvp key in keys)
        {
            attachmentPairs.Add
                (key.attachment, key.attachmentPair);
        }

        ToggleWeaponModCanvas(false);

        railgunCharge = 15;
    }

    private void Update()
    {
        if (railgunCharge < 15 && !weaponManager.guns[weaponManager.activeGun].charging)
        {
            railgunCharge += Time.deltaTime * railgunChargeRate;
        }

        railgunBar.fillAmount = railgunCharge / 15;

        weaponManager.guns[3].currentAmmo = (int)Mathf.Floor(railgunCharge);
    }

    public int GetKeycardLevel()
    {
        return keycardLevel;
    }

    public void SetKeycardLevel(int level)
    {
        keycardLevel = level;
        StartCoroutine(InventoryText($"You obtained a level {level} keycard!", Color.white));
        UpdateKeycard(level - 1);
    }

    void UpdateKeycard(int level)
    {
        foreach (GameObject keycard in keycards)
        {
            keycard.SetActive(false);
        }

        keycards[level].SetActive(true);
    }

    IEnumerator InventoryText(string text, Color color)
    {
        inventoryText.transform.GetChild(0).gameObject.SetActive(true);
        inventoryText.transform.GetChild(1).gameObject.SetActive(true);

        inventoryText.SetActive(true);
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().color = color;

        yield return new WaitForSeconds(5.9f);

        inventoryText.transform.GetChild(0).gameObject.SetActive(false);
        inventoryText.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ToggleWeaponModCanvas(bool toggle)
    {
        for (int i = weaponManager.activeGun == 3 ? 4 : 0; i < gameObject.transform.childCount; i++)
        {
            weaponModCanvas.transform.GetChild(i).gameObject.SetActive(toggle);
        }
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
        foreach (InventorySlot slot in weaponSlots[weaponManager.activeGun - 1].slots)
        {
            slot.slotGunType = gunType;
        }
    }

    public int GetAmmoCount()
    {
        switch (selectedAmmo)
        {
            case AmmoType.STANDARD: return standardAmmo;
            case AmmoType.EXPLOSIVE: return explosiveAmmo;
            case AmmoType.SLOW: return slowAmmo;
            case AmmoType.INCINDIARY: return fireAmmo;
        }

        return 0;
    }

    public int GetAmmoCount(AmmoType selectedAmmo)
    {
        switch (selectedAmmo)
        {
            case AmmoType.STANDARD: return standardAmmo;
            case AmmoType.EXPLOSIVE: return explosiveAmmo;
            case AmmoType.SLOW: return slowAmmo;
            case AmmoType.INCINDIARY: return fireAmmo;
        }

        return 0;
    }

    public void AddAmmo(int ammo, AmmoType ammoType)
    {
        switch (ammoType)
        {
            case AmmoType.STANDARD:
                standardAmmo += ammo;
                break;
            case AmmoType.EXPLOSIVE:
                explosiveAmmo += ammo;
                break;
            case AmmoType.SLOW:
                slowAmmo += ammo;
                break;
            case AmmoType.INCINDIARY:
                fireAmmo += ammo;
                break;
        }
    }

    public void AddRandomItem(int attachment)
    {
        var itemSlot = NextSlot();

        var itemObject = Instantiate(itemPrefab, itemSlot.transform).GetComponent<InventoryItem>();

        if (attachment > availableAttachments.Count)
        {
            itemObject.attachment = railGunAttachment;
        }
        else
        {
            itemObject.attachment = availableAttachments[attachment];
        }

        
        itemObject.UpdateItemStats();
        itemSlot.item = itemObject;
        itemObject.GetComponent<DragDrop>().previousSlot = itemSlot.GetComponent<RectTransform>();
    }

    public InventorySlot NextSlot()
    {
        foreach(InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount < 1)
            {
                return slot;
            }
        }

        return null;
    }

    public bool CheckForEmptySlot()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount < 1)
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleVisibleSlots(bool toggle)
    {
        if (toggle)
        {
            foreach (InventorySlotGroups weaponSlotGroups in weaponSlots)
            {
                foreach (InventorySlot slot in weaponSlotGroups.slots)
                {
                    if (slot.slotGunType == GunType.PISTOL && weaponManager.GetActiveGun() == 1)
                    {
                        slot.transform.parent.gameObject.SetActive(true);
                    }
                    else if (slot.slotGunType == GunType.RIFLE && weaponManager.GetActiveGun() == 2)
                    {
                        slot.transform.parent.gameObject.SetActive(true);
                    }
                    else if (slot.slotGunType == GunType.RAILGUN && weaponManager.GetActiveGun() == 3)
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

    public void UpdateWeaponStats()
    {
        foreach(InventorySlot slot in weaponSlots[weaponManager.activeGun - 1].slots)
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

        if (barrelSlot.transform.childCount < 1) barrelSlot.item = null;
        if (magazineSlot.transform.childCount < 1) magazineSlot.item = null;
        if (gripSlot.transform.childCount < 1) gripSlot.item = null;

        weaponManager.guns[weaponManager.activeGun].changeAmmo((WeaponModScriptableObject)attachmentPairs[selectedAmmoSlot.attachment]);

        if (barrelSlot.item != null) weaponManager.guns[weaponManager.activeGun].changeBarrel((WeaponModScriptableObject)attachmentPairs[barrelSlot.item.attachment]);
        else weaponManager.guns[weaponManager.activeGun].changeBarrel(standardAttachments[0]);

        if (gripSlot.item != null) weaponManager.guns[weaponManager.activeGun].changeGrip((WeaponModScriptableObject)attachmentPairs[gripSlot.item.attachment]);
        else weaponManager.guns[weaponManager.activeGun].changeGrip(standardAttachments[1]);

        if (magazineSlot.item != null) weaponManager.guns[weaponManager.activeGun].changeMag((WeaponModScriptableObject)attachmentPairs[magazineSlot.item.attachment]);
        else weaponManager.guns[weaponManager.activeGun].changeMag(standardAttachments[2]);

        if (magazineSlot.item == null)
        {
            if (prevMag != null)
            {
                AddAmmo(weaponManager.guns[weaponManager.activeGun].currentAmmo, prevAmmo);
                weaponManager.guns[weaponManager.activeGun].currentAmmo = 0;
            }
        }
        else
        {
            if (prevMag != null)
            {
                if (magazineSlot.item.attachment != prevMag)
                {
                    AddAmmo(weaponManager.guns[weaponManager.activeGun].currentAmmo, prevAmmo);
                    weaponManager.guns[weaponManager.activeGun].currentAmmo = 0;
                }
            }
            else
            {
                AddAmmo(weaponManager.guns[weaponManager.activeGun].currentAmmo, prevAmmo);
                weaponManager.guns[weaponManager.activeGun].currentAmmo = 0;
            }
        }

        if (selectedAmmo != prevAmmo)
        {
            for (int i = 0; i < weaponManager.guns.Count; i++)
            {
                AddAmmo(weaponManager.guns[i].currentAmmo, prevAmmo);
                weaponManager.guns[i].currentAmmo = 0;
            }
        }



        if (trashSlot.transform.childCount > 1)
        {
            Destroy(trashSlot.transform.GetChild(1).gameObject);
        }

        weaponManager.guns[weaponManager.activeGun].UpdateWeaponStats();
    }

    public void UpdateWeaponStatsRailgun()
    {
        barrelSlot = weaponSlots[weaponManager.activeGun - 1].slots[0];

        if (barrelSlot.item != null)
        {
            weaponManager.guns[weaponManager.activeGun].changeBarrel((WeaponModScriptableObject)attachmentPairs[barrelSlot.item.attachment]);
        }

        else
        {
            weaponManager.guns[weaponManager.activeGun].changeBarrel(standardRailgunAttachment);
        }
    }

    public void ShootRailgunAmmo()
    {
        railgunCharge -= 1;
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
