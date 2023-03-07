using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItem item;
    Image image;
    public Sprite emptyImage;
    string attachmentName;
    string attachmentDescription;

    public bool isAmmoSlot = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
            eventData.pointerDrag.GetComponent<DragDrop>().itemSlot = GetComponent<InventorySlot>();
        }
    }
}
