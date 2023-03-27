using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amalgamation : EnemyBase
{
    bool attacking = false;
    public float attackKnockbackStrength;
    

    private void Update()
    {
        EnemyUpdate();

        if (GetPlayerDistance() <= _attackRange && !attacking && canAttack)
        {
            StartCoroutine(AttackPlayer());
        }

    }

    IEnumerator AttackPlayer()
    {
        attacking = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.6f);
        attacking = false;
        animator.SetBool("isAttacking", false);

        if (GetPlayerDistance() <= _attackRange + 0.5f)
        {
            player.GetComponent<Player>().TakeDamage(_damage, "Amalgamation");
            
            StartCoroutine(player.GetComponent<Player>().Knockback(1,this.gameObject,attackKnockbackStrength));
        }
    }
    /*
    IEnumerator Knockback(float duration)
    {
        player.GetComponent<PlayerControlsManager>().enabled = false;
        player.GetComponent<Player>().Knockback(this.gameObject, attackKnockbackStrength);
        yield return new WaitForSeconds(duration);
        player.GetComponent<PlayerControlsManager>().enabled = true;
    }
    */
    
}
