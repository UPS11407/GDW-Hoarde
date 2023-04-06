using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class Computer : MonoBehaviour, IInteractible
{
    public string titleText;

    [Tooltip("Watch out for text too long to properly display!")]
    [TextArea(12, 12)]
    public string msgText;
    public TextMeshProUGUI title;
    public TextMeshProUGUI msg;
    public Transform lookAtTransform;
    GameObject cameraObj;
    GameObject temp;
    bool moving = false;
    bool exit = false;
    public bool looking = false;
    Transform startTransform;
    float ratio;
    List<GameObject> UIStuffs = new List<GameObject>();
    GameObject player;

    public bool isLaunch;
    public GameObject button;
    public GameObject rt;
    public GameObject vp;
    Inventory inv;

    public GameObject nuke;

    private void Start()
    {
        cameraObj = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        GetComponentInChildren<Canvas>().worldCamera = cameraObj.GetComponent<Camera>();
        title.text = titleText;
        msg.text = msgText;
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        if (isLaunch) button.SetActive(true);
    }

    public void Interact()
    {
        UIStuffs.Add(GameObject.Find("Inventory UI"));
        UIStuffs.Add(GameObject.Find("HUD"));

        foreach (GameObject obj in UIStuffs)
        {
            obj.SetActive(false);
        }

        temp = cameraObj.transform.parent.gameObject;
        cameraObj.transform.parent = gameObject.transform;
        startTransform = cameraObj.transform;

        cameraObj.GetComponent<CameraRecoil>().enabled = false;
        player.GetComponent<PlayerControlsManager>().playerInput.SwitchCurrentActionMap("Menu");
        player.GetComponent<WeaponManager>().guns[player.GetComponent<WeaponManager>().activeGun].gameObject.SetActive(false);

        player.GetComponent<HeadBob>().enabled = false;

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
        player.GetComponent<PlayerControlsManager>().playerInput.SwitchCurrentActionMap("Player");
        player.GetComponent<WeaponManager>().guns[player.GetComponent<WeaponManager>().activeGun].gameObject.SetActive(true);
        GameObject.Find("WeaponCam").transform.localRotation = Quaternion.Euler(Vector3.zero);
        player.GetComponent<HeadBob>().enabled = true;

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

    public void ButtonPress()
    {
        if(inv.GetKeycardLevel() == 5)
        {
            StartCoroutine(Ronk());
        }
    }

    IEnumerator Ronk()
    {
        rt.SetActive(true);
        vp.SetActive(true);
        nuke.GetComponent<NUKE>().LaunchNuke();

        yield return new WaitForSecondsRealtime(8);

        vp.GetComponent<VideoPlayer>().Pause();

        yield return new WaitForSecondsRealtime(1.5f);

        rt.SetActive(false);
        
        vp.SetActive(false);
    }
}
