using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradingSwitch : MonoBehaviour
{
    public Material noirMat;
    public Material sepiaMat;
    public LUTCamera LUTScript;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (LUTScript.enabled)
            {
                LUTScript.enabled = false;
            } else
            {
                LUTScript.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (LUTScript.m_renderMaterial == sepiaMat)
            {
                LUTScript.m_renderMaterial = noirMat;
            } else
            {
                LUTScript.m_renderMaterial = sepiaMat;
            }
        }
    }
}
