using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float explosionRange;
    public bool isExplosive;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            //Deal Damage to collision
        }
        if (isExplosive)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if(Vector3.Distance(enemy.transform.position, transform.position) < explosionRange){
                    //Deal Damage to enemy
                }
            }
        }

        Destroy(gameObject);
    }
}
