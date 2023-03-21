using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoBoxText : MonoBehaviour
{
    public Vector2 offset;
    Canvas canvas;
    public TMP_Text att_name;
    public TMP_Text att_weapon;
    public TMP_Text att_desc;
    public TMP_Text weaponBulletCount;
    public Inventory inventory;
    new RectTransform transform;
    

    private void Awake()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        transform = GetComponent<RectTransform>();
        ToggleSelf(false);
    }

    private void Update()
    {
        MoveBox();

        Debug.Log(transform.localPosition.x);

        if (transform.localPosition.y > 110)
        {
            transform.pivot = Vector2.up;
        }
        else
        {
            transform.pivot = Vector2.zero;
        }
    }

    public void UpdateBox(string name, string weapon, string desc)
    {
        att_name.text = name;

        att_desc.text = desc;

        weaponBulletCount.text = "Weapon: ";
        att_weapon.text = weapon;
    }

    public void UpdateBox(string name, string weapon, string desc, InventorySlot itemslot)
    {
        att_name.text = name;

        att_desc.text = desc;

        weaponBulletCount.text = "Count: ";
        att_weapon.text = inventory.GetAmmoCount(itemslot.item.attachment.ammoType).ToString();
    }

    public void ToggleSelf(bool toggle)
    {
        att_desc.gameObject.SetActive(toggle);
        att_weapon.gameObject.SetActive(toggle);
        weaponBulletCount.gameObject.SetActive(toggle);
        att_name.gameObject.SetActive(toggle);
        GetComponent<Image>().enabled = toggle;
    }

    void MoveBox()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos + offset);
    }
}
