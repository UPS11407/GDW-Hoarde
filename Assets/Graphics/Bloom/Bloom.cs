using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bloom : MonoBehaviour
{
    public Shader bloomShader;

    [Range(0, 16)]
    [SerializeField] int iterations = 1;
    RenderTexture[] textures = new RenderTexture[16];
    
    [NonSerialized] Material bloom;
    [SerializeField] Material colourGrading;
    // Start is called before the first frame update
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (bloom == null)
        {
            bloom = new Material(bloomShader);
            bloom.hideFlags = HideFlags.HideAndDontSave;
        }


        int width = source.width / 2;
        int height = source.height / 2;

        RenderTextureFormat format = source.format;
        RenderTexture currentDestination = textures[0] = RenderTexture.GetTemporary(width, height, 0, format);
        Graphics.Blit(source, currentDestination, bloom);
        RenderTexture currentSource = currentDestination;
        int i = 1;
        for (; iterations < i; i++)
        {
            width /= 2;
            height /= 2;
            if (height < 2)
            {
                break;
            }
            currentDestination = textures[i] = RenderTexture.GetTemporary(width, height, 0, format);
            Graphics.Blit(currentSource, currentDestination, bloom);
            currentSource = currentDestination;

        }
        for (i -= 2; i >= 0; i--)
        {
            currentDestination = textures[i];
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination, bloom);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }
        Graphics.Blit(currentDestination, destination, bloom);
        Graphics.Blit(currentDestination, destination, colourGrading);
        RenderTexture.ReleaseTemporary(currentSource);
        
    }

}