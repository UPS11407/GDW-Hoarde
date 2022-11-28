using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] Vector3 currentRotation;
    [SerializeField] Vector3 targetRotation;


    /*float xRecoil;
    float yRecoil;
    float zRecoil;
    */

    [SerializeField] float snapValue;
    [SerializeField] float restSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, restSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapValue * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil(float xRecoil, float yRecoil, float zRecoil)
    {
        targetRotation += new Vector3(xRecoil, Random.Range(-yRecoil, yRecoil), Random.Range(-zRecoil, zRecoil));
    }
}
