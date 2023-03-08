using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryAttachment;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public WeaponManager weaponManager;
    public InventoryItem item;

    public bool isAmmoSlot;
    public bool isWeaponSlot;

    public AttachmentType slotAttachmentType;
    public GunType slotGunType;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            item = eventData.pointerDrag.GetComponent<InventoryItem>();

            if (isAmmoSlot || (isWeaponSlot && item.attachmentType != slotAttachmentType) || (isWeaponSlot && item.attachmentWeapon != slotGunType))
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<DragDrop>().previousSlot.anchoredPosition;
                return;
            }

            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
            eventData.pointerDrag.GetComponent<Transform>().localScale = Vector3.one;
            eventData.pointerDrag.GetComponent<DragDrop>().itemSlot = GetComponent<InventorySlot>();
            eventData.pointerDrag.GetComponent<DragDrop>().previousSlot = eventData.pointerDrag.GetComponent<RectTransform>().parent.GetComponent<RectTransform>();
        }
    }
}
