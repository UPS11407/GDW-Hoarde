using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitPuddle : MonoBehaviour
{
    public float _damage;
    bool isOnPuddle;
    GameObject player;
    public float _duration;
    private void Start()
    {
        //Destroy(this.gameObject, _duration);
    }

    private void FixedUpdate()
    {
        if (isOnPuddle)
        {
            player.GetComponent<Player>().TakeDamageOverTime(_damage * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            isOnPuddle = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOnPuddle = false;
        }
    }
}
