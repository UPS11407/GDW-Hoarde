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
    TrashSlot trashSlot;

    Transform inventoryTransform;
    Inventory inventory;
    public RectTransform previousSlot;

    Color emptyColor = Color.gray;

    public InfoBoxText infoBox;
    
    private void Awake()
    {
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
            trans.anchoredPosition = previousSlot.localPosition;
            trans.SetParent(previousSlot);
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
            inventory.selectedAmmo = System.Enum.Parse<AmmoType>(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>().ammoType, true);
            inventory.selectedAmmoSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>();
            Debug.Log(inventory.selectedAmmo);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot slot = trans.parent.GetComponent<InventorySlot>();
        InventorySlot prevSlot = eventData.pointerDrag.GetComponent<DragDrop>().previousSlot;

        if (slot.isAmmoSlot || (slot.isWeaponSlot && eventData.pointerDrag.GetComponent<InventoryItem>().attachmentType != slot.slotAttachmentType) 
            || (slot.isWeaponSlot && eventData.pointerDrag.GetComponent<InventoryItem>().attachmentWeapon != slot.slotGunType))
            
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<DragDrop>().previousSlot.anchoredPosition;
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
            infoBox.UpdateBox(itemSlot.item.attachment.attachmentName, weapon, itemSlot.item.attachment.description, true);
        }
        else
        {
            infoBox.UpdateBox(itemSlot.item.attachment.attachmentName, weapon, itemSlot.item.attachment.description, false);
        }

        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.ToggleSelf(false);
    }
}
