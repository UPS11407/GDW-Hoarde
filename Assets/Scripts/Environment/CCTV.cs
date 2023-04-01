using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    GameObject player;
    Vector3 restRotation;
    float maxXRotation = 60.0f;
    float maxYRotation = 90.0f;
    float maxRotation = 75.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        restRotation = transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit rayhit;
        Vector3 dir2P = (player.transform.position - gameObject.transform.position).normalized;
        Physics.Raycast(origin: transform.position, direction: dir2P, out rayhit, maxDistance: 30.0f, layerMask: 1 << 9);
        //Debug.DrawRay(start: Camera.main.transform.position, dir: Camera.main.transform.forward * 10, Color.red, 60);
        Debug.Log(Vector3.Angle(restRotation, dir2P));
        if (rayhit.collider != null && Vector3.Angle(restRotation, dir2P) <= maxRotation)
        {
            

            Quaternion target = Quaternion.LookRotation(dir2P);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, 0.1f);
            //Vector3 currentRotation = transform.localRotation.eulerAngles;
            //transform.rotation = Quaternion.Euler(-Mathf.Clamp(currentRotation.x, -maxXRotation, maxXRotation), currentRotation.y, 0);
            //Debug.Log(currentRotation);
            
            
        }
        else Debug.Log("Not Seen");
    }
}
