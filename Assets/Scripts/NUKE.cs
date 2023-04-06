using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NUKE : MonoBehaviour
{
    public GameObject fire;
    public Transform firePos;

    public void LaunchNuke()
    {
        var fir = Instantiate(fire, firePos.position, firePos.rotation);
        StartCoroutine(bomba());
    }

    IEnumerator bomba()
    {
        yield return new WaitForSecondsRealtime(9.4f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneControl.ChangeScene("WIN");
    }
}
