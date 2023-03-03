using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryAttachment;

public class InventoryItem : MonoBehaviour
{
    public List<InventoryAttachment> itemTypes;
    public List<InventoryAttachment> ammoTypes;


    public InventoryAttachment attachment;
    GunType attachmentWeapon;
    AttachmentType attachmentType;
    string attachmentModifier;
    bool isAmmo;
    string ammoType;
}
