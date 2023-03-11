using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    

    private void Awake()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        ToggleSelf(false);
    }

    private void Update()
    {
        MoveBox();
    }

    public void UpdateBox(string name, string weapon, string desc, bool isAmmo)
    {
        att_name.text = name;
        att_weapon.text = weapon;   
        att_desc.text = desc;

        if (isAmmo)
        {
            weaponBulletCount.text = "Count: ";
        }
        else
        {
            weaponBulletCount.text = "Weapon: ";
        }
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
