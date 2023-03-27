using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathUIFiller : MonoBehaviour
{
    Counters counter;
    public TextMeshProUGUI killCount;
    public TextMeshProUGUI timeSurvived;
    public TextMeshProUGUI deathResult;
    void Start()
    {
        counter = GameObject.Find("Counter").GetComponent<Counters>();

        killCount.text = counter.GetKills().ToString();
        //Debug.Log(counter.GetTime());
        timeSurvived.text = (counter.GetTime() / 60f).ToString("0.##") + " minutes";
        deathResult.text = counter.GetKilledBy().ToString();
        Destroy(counter.gameObject);
    }
}
