using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerExplosion : MonoBehaviour
{
    public float _explosionRange;
    public float _damage;
    public AnimationCurve damageCurve;
    // Start is called before the first frame update
    private void Start()
    {


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRange);
        foreach (var hitCollider in hitColliders)
        {
            float damageMod = damageCurve.Evaluate(Mathf.Clamp01(1 - (Vector3.Distance(gameObject.transform.position,hitCollider.transform.position) / _explosionRange)));
            
            if(hitCollider.gameObject.tag == "Player")
            {
                hitCollider.GetComponent<Player>().TakeDamage(_damage * damageMod);
                Debug.Log("HIT PLAYER");
                Debug.Log(_damage * damageMod);
            }
                
            if(hitCollider.gameObject.tag == "Enemy")
            {
                hitCollider.GetComponent<EnemyBase>().TakeDamage(_damage * damageMod);
            }

            

        }
        Destroy(this.gameObject, 5.0f);

    }



}
