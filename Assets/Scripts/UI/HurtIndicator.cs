using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtIndicator : MonoBehaviour
{
    public Image indicator;
    public float maxTimeToFade;
    public float hpToIndicatorStay;
    public float fadePercentToStay;
    float timeToFade;
    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        float fadePercent = timeToFade / maxTimeToFade;

        if (player.GetHP() > player.maxHP * (hpToIndicatorStay / 100) || fadePercent > (fadePercentToStay / 100))
        {
            timeToFade -= Time.deltaTime;
        }

        indicator.color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, fadePercent);
    }

    public void Hurt()
    {
        timeToFade = maxTimeToFade;
    }
}
