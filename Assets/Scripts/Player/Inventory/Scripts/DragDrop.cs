using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform trans;
    Canvas canvas;
    CanvasGroup canvasGroup;
    public InventorySlot itemSlot;
    TrashSlot trashSlot;

    Transform inventoryTransform;
    RectTransform previousSlot;

    private void Awake()
    {
        itemSlot = GetComponent<InventorySlot>();
        trans = GetComponent<RectTransform>();
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryTransform = GameObject.Find("Inventory").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.isAmmoSlot)
        {
            return;
        }

        previousSlot = trans.parent.GetComponent<RectTransform>();

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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.isAmmoSlot)
        {

        }
    }
}
