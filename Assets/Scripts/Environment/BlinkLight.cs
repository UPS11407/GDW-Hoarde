using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    public GameObject camLight;
    public float blinkInterval;
    float interval;

    void Update()
    {
        if (!GetComponent<CCTV>().seesPlayer)
        {
            camLight.SetActive(false);
            return;
        }

        if (interval > blinkInterval)
        {
            if (camLight.activeSelf)
            {
                camLight.SetActive(false);
            }
            else
            {
                camLight.SetActive(true);
            }

            interval = 0;
        }

        interval += Time.deltaTime;
    }
}
