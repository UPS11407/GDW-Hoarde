using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentMesh : MonoBehaviour
{
    enum AttachmentMeshType
    {
        Barrel,
        Magazine
    };
    [SerializeField] AttachmentMeshType attachmentType;
    [SerializeField] WeaponModScriptableObject attachment;
    [SerializeField] GameObject gun;
    private void Update()
    {
        if (gun.active)
        {
            if (attachmentType == AttachmentMeshType.Barrel)
            {
                if(attachment == gun.GetComponent<Gun>().barrelMod)
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<Renderer>().enabled = false;

                }
            }
            else if (attachmentType == AttachmentMeshType.Magazine)
            {
                if(attachment == gun.GetComponent<Gun>().magMod)
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<Renderer>().enabled = false;

                }
            }
        }
        
    }
}
