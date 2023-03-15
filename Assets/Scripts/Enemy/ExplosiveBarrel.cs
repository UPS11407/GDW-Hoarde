using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : EnemyBase
{
    [SerializeField] GameObject fireEffect;
    [SerializeField] float _explosionRange;
    [SerializeField] AnimationCurve damageCurve;


    private void Update()
    {
        CheckIfDead();
    }
    public override void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            Explode();
        }
    }
    void Explode()
    {
        GameObject boom = (GameObject)Instantiate(fireEffect, transform.position, transform.rotation);
        boom.GetComponent<BoomerExplosion>()._explosionRange = _explosionRange;
        boom.GetComponent<BoomerExplosion>()._damage = _damage;
        boom.GetComponent<BoomerExplosion>().damageCurve = damageCurve;
        Destroy(boom, 2.5f);
        Destroy(this.gameObject);
    }
}
