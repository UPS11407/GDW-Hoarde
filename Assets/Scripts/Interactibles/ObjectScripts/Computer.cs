using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractible
{
    [TextArea(13, 13)]
    public string interactText;
    public Transform lookAtTransform;
    bool interactable = true;
    GameObject cameraObj;
    GameObject temp;
    bool moving = false;
    bool exit = false;
    bool looking = false;
    Transform startTransform;
    float ratio;
    List<GameObject> UIStuffs = new List<GameObject>();

    private void Start()
    {
        cameraObj = GameObject.Find("Main Camera");
        GetComponentInChildren<Canvas>().worldCamera = cameraObj.GetComponent<Camera>();
    }

    public void Interact()
    {
        UIStuffs.Add(GameObject.Find("Inventory UI"));
        UIStuffs.Add(GameObject.Find("HUD"));

        foreach(GameObject obj in UIStuffs)
        {
            obj.SetActive(false);
        }

        temp = cameraObj.transform.parent.gameObject;
        cameraObj.transform.parent = gameObject.transform;
        startTransform = cameraObj.transform;

        cameraObj.GetComponent<CameraRecoil>().enabled = false;
        GameObject.Find("Player").GetComponent<PlayerControlsManager>().movementLock = true;
        GameObject.Find("Player").GetComponent<PlayerControlsManager>().enableLook = false;

        ratio = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        looking = false;
        moving = true;
        exit = false;
    }

    public void ExitMenu()
    {
        foreach (GameObject obj in UIStuffs)
        {
            obj.SetActive(true);
        }

        UIStuffs.Clear();
        ratio = 0;

        cameraObj.transform.parent = temp.transform;
        startTransform = cameraObj.transform;

        cameraObj.GetComponent<CameraRecoil>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerControlsManager>().movementLock = false;
        GameObject.Find("Player").GetComponent<PlayerControlsManager>().enableLook = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

        looking = false;
        moving = true;
        exit = true;
    }

    void FixedUpdate()
    {
        if (moving)
        {
            ratio += 0.05f;
            if (ratio > 1f) {
                moving = false;
                if (exit)
                {
                    looking = false;
                }
                else
                {
                    looking = true;
                }
                
            } 
            else
            {
                if (!exit)
                {
                    cameraObj.transform.position = Vector3.Lerp(startTransform.position, lookAtTransform.position, ratio);
                    cameraObj.transform.rotation = Quaternion.Lerp(startTransform.rotation, lookAtTransform.rotation, ratio);
                }
                else
                {
                    cameraObj.transform.position = Vector3.Lerp(startTransform.position, temp.transform.position, ratio);
                    cameraObj.transform.rotation = Quaternion.Lerp(startTransform.rotation, Quaternion.Euler(0, 0, 0), ratio);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (looking)
        {
            Time.timeScale = 0;
            foreach (GameObject obj in UIStuffs)
            {
                obj.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
