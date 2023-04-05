using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 hitPoint;
    public float laserDuration;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        StartCoroutine(SpawnLine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnLine()
    {
        //line.gameObject.layer = 10;
        float time = 0;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hitPoint);

        Color laserColor = line.material.GetColor("_EmissionColor");
        Destroy(line.gameObject, laserDuration);
        while (time <= laserDuration)
        {
            line.material.color = Color.Lerp(new Color(line.material.color.r, line.material.color.g, line.material.color.b, 1), new Color(line.material.color.r, line.material.color.g, line.material.color.b, 0), time / laserDuration);
            line.material.SetColor("_EmissionColor", laserColor * Mathf.Pow(2.0f, Mathf.Lerp(3, -10, time / laserDuration)));
            time += Time.deltaTime;

            yield return null;
        }



    }
}
