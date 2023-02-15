using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthbar;

    public void SetHealth(float healthPercentage)
    {
        healthbar.fillAmount = healthPercentage;

        Debug.Log(healthPercentage);

        if (healthPercentage * 100 > 50)
        {
            healthbar.color = Color.Lerp(Color.yellow, Color.green, (healthPercentage * 100 - 50) / 50);
        }
        else
        {
            healthbar.color = Color.Lerp(Color.red, Color.yellow, healthPercentage * 100 / 60);
        }
    }
}
