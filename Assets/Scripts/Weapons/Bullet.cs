using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 3)
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
