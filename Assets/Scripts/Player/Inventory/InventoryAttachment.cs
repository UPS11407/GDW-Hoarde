using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "InventoryAttachment", menuName = "ScriptableObjects/InventoryAttachment")]
public class InventoryAttachment : ScriptableObject
{
    public enum GunType { PISTOL, RIFLE, RAILGUN };

    public Image image;
    public GunType gunType;
    public string description;
}
