using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float strength)
    {
        Vector3 originalPos = transform.localPosition;
        float i = 0.0f;

        while (i < duration)
        {
            Debug.Log("Happen");
            float xPos = Random.Range(-1.0f, 1.0f) * strength;
            float yPos = Random.Range(-1.0f, 1.0f) * strength;
            float zPos = Random.Range(-1.0f, 1.0f) * strength;

            transform.localPosition = new Vector3(xPos, yPos, zPos);

            i += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
