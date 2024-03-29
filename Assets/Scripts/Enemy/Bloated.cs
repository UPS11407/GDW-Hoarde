using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloated : EnemyBase 
{
    //bool attacking = false;
    [SerializeField] float _explosionRange;
    [SerializeField] float _explosionDelay;
    [SerializeField] GameObject fireEffect;
    private Material _material;
    [SerializeField] AnimationCurve damageCurve;  
    bool triggered = false;

    private void Start()
    {
        _material = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }
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
        PufferFish();
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
        if (GetPlayerDistance() <= _attackRange || NavMeshRemainingDistance(agent.path.corners) >= chaseRange)
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

    void PufferFish()
    {
        for (float i = 0; i <= 0.4; i += Time.deltaTime)
        {
            _material.SetFloat("_Amount", i);
            _material.SetColor("_Color", Color.red);
        }
    }

}
