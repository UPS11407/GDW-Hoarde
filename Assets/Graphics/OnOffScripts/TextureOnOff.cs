using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOnOff : MonoBehaviour
{

    public Material shaderMat;
    public Material standardMat;
    public Material[] materials;
    private void Start()
    {
        materials = GetComponent<Renderer>().materials;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("Switch");
            if (materials[0] == shaderMat)
            {
                materials[0] = standardMat;
            } else
            {
                materials[0] = shaderMat;
            }
            GetComponent<Renderer>().materials = materials;
        }
    }
}
