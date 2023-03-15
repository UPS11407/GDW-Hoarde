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
    Light lightComponent;
    public Material onMaterial;
    public Material offMaterial;

    private void Awake()
    {
        lightComponent = gameObject.GetComponent<Light>();
    }

    private void Update()
    {
        if(Time.time >= changeTime)
        {
            lightComponent.enabled = !lightComponent.enabled;
            if (lightComponent.enabled)
            {
                timeOn = Random.Range(timeOnMin, timeOnMax);
                changeTime = Time.time + timeOn;
                ToggleMat(true);
            } else
            {
                timeOff = Random.Range(timeOffMin, timeOffMax);
                changeTime = Time.time + timeOff;
                ToggleMat(false);
            }
        }
    }

    void ToggleMat(bool toggle)
    {
        var parentMesh = lightComponent.transform.parent.GetComponent<MeshRenderer>();

        if (toggle)
        {
            parentMesh.material = onMaterial;
        }
        else
        {
            parentMesh.material = offMaterial;
        }
    }
}
