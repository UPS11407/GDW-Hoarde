using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Image staminabar;

    public void SetStamina(float staminaPercentage)
    {
        staminabar.fillAmount = staminaPercentage;

        if (staminaPercentage >= 1)
        {
            staminabar.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            staminabar.gameObject.transform.parent.gameObject.SetActive(true);
        }
    }
}
