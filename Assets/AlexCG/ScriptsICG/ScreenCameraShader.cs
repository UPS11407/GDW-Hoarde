using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCameraShader : MonoBehaviour
{
    //public Shader awesomeShader = null;
    public Material m_renderMaterial;
    public Material m_renderMaterial2;
    public Material m_renderMaterial3;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_renderMaterial);

        if (Input.GetKey(KeyCode.Alpha0))
        {
            Graphics.Blit(source, destination, m_renderMaterial2);
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            Graphics.Blit(source, destination, m_renderMaterial3);
        }
    }
}