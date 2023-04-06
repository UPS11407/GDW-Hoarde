using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leo : NPCBehavior
{
    public GameObject upperFloorDoor;
    public GameObject toolBox;
    public GameObject keyCard;
    GameObject inventoryText;

    // Start is called before the first frame update
    void Start()
    {
        inventoryText = GameObject.Find("InventoryText");
        Startup();
    }

    // Update is called once per frame
    public override void UpdateHUD()
    {
        if (state == 0 && dialogueState == 8)
        {
            toolBox.SetActive(true);
        }
        if (state == 0 && dialogueState == 1)
        {
            player.GetComponent<Player>().TakeDamage(1, "Thrown laptop");
        }
        if (state == 1 && dialogueState == 6)
        {
            player.GetComponent<Player>().hasNV = true;
            StartCoroutine(InventoryText($"Press {GameObject.Find("Player").GetComponent<Player>().GetBindingReadable(3)} for NightVision", Color.yellow));
            keyCard.SetActive(true);

        }

        base.UpdateHUD();
    }

    IEnumerator InventoryText(string text, Color color)
    {
        inventoryText.transform.GetChild(0).gameObject.SetActive(true);
        inventoryText.transform.GetChild(1).gameObject.SetActive(true);

        inventoryText.SetActive(true);
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        inventoryText.transform.GetChild(1).GetComponent<TMP_Text>().color = color;

        yield return new WaitForSeconds(4.9f);

        inventoryText.transform.GetChild(0).gameObject.SetActive(false);
        inventoryText.transform.GetChild(1).gameObject.SetActive(false);
    }
}
