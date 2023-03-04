using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "InventoryAttachment", menuName = "ScriptableObjects/InventoryAttachment")]
public class InventoryAttachment : ScriptableObject
{
    public enum GunType { NULL, PISTOL, RIFLE, RAILGUN };
    public enum AttachmentType { BARREL, MAGAZINE, GRIP, AMMO };
    public enum BarrelType { NULL, SNIPER, SHOTGUN, SPEEDCHARGE }
    public enum AmmoType { NULL, EXPLOSIVE, INCINDIARY, SLOW };
    public enum MagazineType { NULL, HIGHCAPACITY, QUICKRELOAD, CALICO };
    public enum GripType { NULL, BURST, AUTO };

    public string attachmentName;
    public Sprite image;
    public GunType gunType;
    public string description = "Sample Text";
    public AttachmentType attachmentType;
    public BarrelType barrelType;
    public MagazineType magazineType;
    public GripType gripType;
    public AmmoType ammoType;
}
