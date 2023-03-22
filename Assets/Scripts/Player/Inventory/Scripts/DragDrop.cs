using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryAttachment;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform trans;
    Canvas canvas;
    CanvasGroup canvasGroup;
    public InventorySlot itemSlot;

    Transform inventoryTransform;
    Inventory inventory;
    public RectTransform previousSlot;

    Color emptyColor = Color.gray;

    public InfoBoxText infoBox;

    private void Awake()
    {
        previousSlot = transform.parent.GetComponent<RectTransform>();

        infoBox = GameObject.Find("Info Box").GetComponent<InfoBoxText>();
        trans = GetComponent<RectTransform>();
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryTransform = GameObject.Find("Inventory").transform;
        inventory = inventoryTransform.GetComponent<Inventory>();

        if (trans.parent != inventoryTransform)
        {
            itemSlot = trans.parent.GetComponent<InventorySlot>();
        }
    }

    private void Start()
    {
        foreach (InventorySlot slot in inventory.ammoSlots)
        {
            if (System.Enum.Parse<AmmoType>(slot.item.ammoType, true) != inventory.selectedAmmo)
            {
                slot.item.gameObject.GetComponent<Image>().color = emptyColor;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.isAmmoSlot)
        {
            return;
        }

        previousSlot = trans.parent.GetComponent<RectTransform>();

        foreach (InventorySlot slot in inventory.weaponSlots[inventory.weaponManager.activeGun].slots)
        {
            if (slot.transform.childCount > 0) slot.transform.GetChild(0).GetComponent<Image>().color = emptyColor;
        }

        foreach (InventorySlot slot in inventory.weaponSlots[inventory.weaponManager.activeGun].slots)
        {
            if (slot.slotGunType == GunType.NULL || slot.slotGunType == itemSlot.item.attachmentWeapon)
            {
                if (slot.slotAttachmentType == itemSlot.item.attachmentType)
                {
                    slot.GetComponent<Image>().color = Color.white;
                    if (slot.transform.childCount > 0) slot.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                }
            }
        }

        trans.SetParent(inventoryTransform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.isAmmoSlot)
        {
            return;
        }

        trans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.isAmmoSlot)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (transform.parent == inventoryTransform)
        {
            trans.SetParent(previousSlot);
            trans.localPosition = Vector2.zero;
        }

        foreach (InventorySlot slot in inventory.weaponSlots[inventory.weaponManager.activeGun].slots)
        {
            slot.GetComponent<Image>().color = emptyColor;
            if (slot.transform.childCount > 0) slot.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.isAmmoSlot)
        {
            foreach (InventorySlot slot in inventory.ammoSlots)
            {
                slot.item.GetComponent<Image>().color = emptyColor;
            }

            eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>().color = Color.white;
            var ammoSelect = System.Enum.Parse<AmmoType>(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>().ammoType, true);
            inventory.selectedAmmo = System.Enum.Parse<AmmoType>(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>().ammoType, true);
            inventory.selectedAmmoSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot slot = trans.parent.GetComponent<InventorySlot>();
        InventorySlot prevSlot = eventData.pointerDrag.GetComponent<DragDrop>().previousSlot.GetComponent<InventorySlot>();

        var draggedAttachmentType = eventData.pointerDrag.GetComponent<InventoryItem>().attachmentType;
        var draggedAttachmentWeapon = eventData.pointerDrag.GetComponent<InventoryItem>().attachmentWeapon;

        if (prevSlot.isAmmoSlot)
        {
            return;
        }

        if (
            slot.isAmmoSlot 
            || (slot.isWeaponSlot && draggedAttachmentType != slot.slotAttachmentType)
            || (slot.isWeaponSlot && draggedAttachmentWeapon != slot.slotGunType)
            )
            
        {
            eventData.pointerDrag.GetComponent<RectTransform>().localPosition = Vector2.zero;
            return;
        }

        if (
            (prevSlot.isWeaponSlot && GetComponent<InventoryItem>().attachmentType != prevSlot.slotAttachmentType)
            || (prevSlot.isWeaponSlot && GetComponent<InventoryItem>().attachmentWeapon != prevSlot.slotGunType)
            )
        {
            

            foreach (InventorySlot inventorySlot in inventory.inventorySlots)
            {
                if (inventorySlot.transform.childCount < 1)
                {
                    
                    eventData.pointerDrag.transform.SetParent(inventorySlot.transform);
                    eventData.pointerDrag.GetComponent<RectTransform>().localPosition = Vector2.zero;
                    eventData.pointerDrag.GetComponent<RectTransform>().localScale = Vector3.one;
                    eventData.pointerDrag.GetComponent<DragDrop>().previousSlot = inventorySlot.GetComponent<RectTransform>();
                    return;
                }
            }

            eventData.pointerDrag.GetComponent<RectTransform>().localPosition = Vector2.zero;
            eventData.pointerDrag.GetComponent<RectTransform>().localScale = Vector3.one;
            return;
        }

        eventData.pointerDrag.GetComponent<RectTransform>().SetParent(previousSlot);
        GetComponent<RectTransform>().SetParent(eventData.pointerDrag.GetComponent<DragDrop>().previousSlot);

        GetComponent<RectTransform>().localPosition = Vector2.zero;
        eventData.pointerDrag.GetComponent<RectTransform>().localPosition = Vector2.zero;

        previousSlot = trans.parent.GetComponent<RectTransform>();
        eventData.pointerDrag.GetComponent<DragDrop>().previousSlot = eventData.pointerDrag.GetComponent<DragDrop>().trans.parent.GetComponent<RectTransform>();

        trans.localScale = Vector3.one;
        eventData.pointerDrag.GetComponent<DragDrop>().trans.localScale = Vector3.one;

        if (slot.isTrashSlot)
        {
            inventory.availableAttachments.Add(itemSlot.item.attachment);
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoBox.ToggleSelf(true);

        string weapon = "";

        switch (itemSlot.item.attachment.gunType)
        {
            case GunType.PISTOL:
                weapon = "Pistol";
                break;
            case GunType.RIFLE:
                weapon = "Rifle";
                break;
            case GunType.RAILGUN:
                weapon = "Rail Gun";
                break;
        }

        if (itemSlot.isAmmoSlot)
        {
            infoBox.UpdateBox(itemSlot.item.attachment.attachmentName, weapon, itemSlot.item.attachment.description, itemSlot);
        }
        else
        {
            infoBox.UpdateBox(itemSlot.item.attachment.attachmentName, weapon, itemSlot.item.attachment.description);
        }

        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.ToggleSelf(false);
    }
}
