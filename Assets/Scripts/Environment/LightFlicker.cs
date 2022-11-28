using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    float timeOn;
    [SerializeField] float timeOnMax = 1.5f;
    [SerializeField] float timeOnMin = 0.25f;
    float timeOff;
    [SerializeField] float timeOffMax = 2.5f;
    [SerializeField] float timeOffMin = 0.5f;
    float changeTime;
    public Light light;
    private void Awake()
    {
        light = gameObject.GetComponent<Light>();
    }
    private void Update()
    {
        if(Time.time >= changeTime)
        {
            light.enabled = !light.enabled;
            if (light.enabled)
            {
                timeOn = Random.Range(timeOnMin, timeOnMax);
                changeTime = Time.time + timeOn;
            } else
            {
                timeOff = Random.Range(timeOffMin, timeOffMax);
                changeTime = Time.time + timeOff;
            }
        }
    }
}
