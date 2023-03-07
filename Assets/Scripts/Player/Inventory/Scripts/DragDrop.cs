using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    RectTransform trans;
    Canvas canvas;
    CanvasGroup canvasGroup;
    public InventorySlot itemSlot;

    private void Awake()
    {
        itemSlot = GetComponent<InventorySlot>();
        trans = GetComponent<RectTransform>();
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot != null && !itemSlot.isAmmoSlot)
        {
            return;
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot != null && !itemSlot.isAmmoSlot)
        {
            return;
        }

        trans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }
}
