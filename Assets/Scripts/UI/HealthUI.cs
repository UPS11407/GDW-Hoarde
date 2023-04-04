using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthbar;
    public Image healCharge;

    public void SetHealth(float healthPercentage)
    {
        healthbar.fillAmount = healthPercentage;
    }

    public void SetCharge(float healthPercentage)
    {
        healCharge.fillAmount = healthPercentage;
    }
}
