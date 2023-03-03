using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthChargeUI : MonoBehaviour
{
    public List<Sprite> chargeImages;

    public Image image;

    public void SetCharge(int healthPercentage)
    {
        image.overrideSprite = chargeImages[healthPercentage];
    }
}
