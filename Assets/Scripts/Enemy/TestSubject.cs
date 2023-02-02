using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubject : EnemyBase
{
    bool attacking = false;
    

    private void Update()
    {
        EnemyUpdate();

        if(GetPlayerDistance() <= _attackRange & !attacking)
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
            player.GetComponent<Player>().TakeDamage(_damage);
        }
    }
}
