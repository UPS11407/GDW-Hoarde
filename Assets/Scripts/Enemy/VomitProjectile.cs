using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitProjectile : MonoBehaviour
{
    public float _directDamage;
    public float _puddleDamage;
    [SerializeField] GameObject vomitPuddlePrefab;
    [SerializeField] float puddleDuration;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(_directDamage);

        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, float.MaxValue))
            {
                GameObject vomitPuddle;
                vomitPuddle = Instantiate(vomitPuddlePrefab, transform.position, Quaternion.Euler(0, 0, 0));
                vomitPuddle.GetComponent<VomitPuddle>()._damage = _puddleDamage;
                Destroy(vomitPuddle, puddleDuration);
                //vomitPuddle.GetComponent<VomitPuddle>()._duration = puddleDuration;
            }
            Destroy(this.gameObject);
        }
    }
}
