using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloated : EnemyBase 
{
    //bool attacking = false;
    [SerializeField] float _explosionRange;
    [SerializeField] float _explosionDelay;
    [SerializeField] GameObject fireEffect;

    [SerializeField] AnimationCurve damageCurve;  
    bool triggered = false;
    

    private void Update()
    {
        EnemyUpdate();

        if (GetPlayerDistance() <= _attackRange)
        {
            StartCoroutine(AoEAttack());
        }
    }
    
    public override void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            dropVal = Random.Range(0, 100);

            if (dropVal < healDropChance)
            {
                DropPickup();
            }
            agent.enabled = false;
            Attack();
        }
    }


    IEnumerator AoEAttack()
    {
        agent.speed = 0;
        triggered = true;
        //attacking = true;
        //animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(_explosionDelay);
        //attacking = false;
        //animator.SetBool("isAttacking", false);

        Attack();
    }

    public void Attack()
    {

        

        


        GameObject boom = (GameObject)Instantiate(fireEffect, transform.position, transform.rotation);
        boom.GetComponent<BoomerExplosion>()._explosionRange = _explosionRange;
        boom.GetComponent<BoomerExplosion>()._damage = _damage;
        boom.GetComponent<BoomerExplosion>().damageCurve = damageCurve;
        Destroy(boom, 2.5f);
        Destroy(this.gameObject);
    }

    public override void ChasePlayer()
    {
        if (GetPlayerDistance() <= _attackRange)
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
        }
        else if (triggered == false)
        {
            animator.SetBool("isWalking", true);
            agent.isStopped = false;
        }

        if (agent.destination != player.transform.position)
        {
            agent.ResetPath();
            agent.SetDestination(player.transform.position);
        }
    }


}
