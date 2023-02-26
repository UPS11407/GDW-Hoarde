using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubject : EnemyBase
{
    bool attacking = false;
    public bool isDecalOff = true;
    [SerializeField] GameObject character;
    
    private void Start()
    {
        
        switch (Random.Range(0, 8))
        {
            case 8:
                _speed *= 4.5f;
                break;
            case 7:
                _speed *= 3.0f;
                break;
            case 6:

            case 5:
                _speed *= 1.5f;
                break;
            
        }
        UpdateSpeed();
    }

    private void Update()
    {
        EnemyUpdate();

        if(GetPlayerDistance() <= _attackRange & !attacking)
        {
            StartCoroutine(AttackPlayer());
        }

        DamagedDecalOn();
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

    void DamagedDecalOn()
    {
        if(_maxHP > currentHP && isDecalOff)
        {
            Debug.Log("Boom");
            
            character.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_ShowDecal1", 1.0f);
            isDecalOff = false;
        }
    }
}
