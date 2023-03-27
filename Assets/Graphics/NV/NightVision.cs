using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightVision : MonoBehaviour
{
    //[SerializeField] Material m_renderMaterial;
    [SerializeField] Material scanLineMaterial;


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Graphics.Blit(source, destination, m_renderMaterial);
        Graphics.Blit(source, destination, scanLineMaterial);

    }
}
