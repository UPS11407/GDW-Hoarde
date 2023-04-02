using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedPP : MonoBehaviour
{
    [SerializeField] Material chromaticDistortion;



    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Graphics.Blit(source, destination, m_renderMaterial);
        Graphics.Blit(source, destination, chromaticDistortion);

    }
}
