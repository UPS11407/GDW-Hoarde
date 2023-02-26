using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminationControl : MonoBehaviour
{
    [SerializeField] List<GameObject> illuminatedObjects;
    [SerializeField] List<Material> materials;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (GameObject gameObject in illuminatedObjects)
            {
                gameObject.GetComponent<MeshRenderer>().material = materials[0];
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (GameObject gameObject in illuminatedObjects)
            {
                gameObject.GetComponent<MeshRenderer>().material = materials[1];
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (GameObject gameObject in illuminatedObjects)
            {
                gameObject.GetComponent<MeshRenderer>().material = materials[2];
            }
        }
    }


}
