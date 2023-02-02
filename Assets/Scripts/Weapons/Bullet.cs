using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float explosionRange;
    public bool isExplosive;
    [SerializeField] GameObject fireEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
        }

        if (isExplosive)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position,explosionRange, 3);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.GetComponent<EnemyBase>().TakeDamage(damage);
            }
            GameObject boom = (GameObject)Instantiate(fireEffect, transform.position, transform.rotation);
            Destroy(boom, 2.5f);


        }

        Destroy(gameObject);
    }
}
