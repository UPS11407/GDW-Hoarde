using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstPersonCameraScript : MonoBehaviour
{
    [SerializeField] float Sensitivity;
    Quaternion quats;
    [SerializeField] float interactRange;
    [SerializeField] LayerMask interactibleMask;
    [SerializeField] GameObject interactText;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        /*
        if (transform.parent.GetComponent<PlayerControls>().speed != transform.parent.GetComponent<PlayerControls>().originalSpeed)
        {
            GetComponent<Camera>().fieldOfView = 65;

        } else
        {
            GetComponent<Camera>().fieldOfView = 60;

        }
        */
        CheckIfInteractible();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -Sensitivity * Time.unscaledDeltaTime, 0, 0));
        transform.parent.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * Sensitivity * Time.unscaledDeltaTime, 0));
        quats = transform.rotation;
        quats.eulerAngles = new Vector3(quats.eulerAngles.x, quats.eulerAngles.y, 0);
        transform.rotation = quats;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.parent.eulerAngles.y, transform.eulerAngles.z);
    }

    void CheckIfInteractible()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, interactibleMask))
        {
            interactText.SetActive(true);
            interactText.GetComponent<TextMeshProUGUI>().text = $"(E) - {hit.transform.name}";
        }
        else
        {
            interactText.SetActive(false);
        }
    }
}
