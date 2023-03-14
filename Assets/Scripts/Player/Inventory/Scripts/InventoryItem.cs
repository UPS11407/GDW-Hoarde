using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryAttachment;

public class InventoryItem : MonoBehaviour
{
    public Sprite emptyImage;
    public InventoryAttachment attachment;
    public GunType attachmentWeapon;
    public AttachmentType attachmentType;
    public string attachmentModifier;
    
    public bool isAmmo;
    public string ammoType;

    private void Awake()
    {
        UpdateItemStats();
    }

    public void UpdateItemStats()
    {
        if (attachment == null)
        {
            GetComponent<Image>().sprite = emptyImage;
        }
        else
        {
            GetComponent<Image>().sprite = attachment.image;
        }

        attachmentType = attachment.attachmentType;
        attachmentWeapon = attachment.gunType;

        switch (attachmentType)
        {
            case AttachmentType.BARREL:
                attachmentModifier = attachment.barrelType.ToString();
                break;

            case AttachmentType.GRIP:
                attachmentModifier = attachment.gripType.ToString();
                break;

            case AttachmentType.MAGAZINE:
                attachmentModifier = attachment.magazineType.ToString();
                break;

            case AttachmentType.AMMO:
                ammoType = attachment.ammoType.ToString();
                isAmmo = true;
                break;
        }

    }
}
