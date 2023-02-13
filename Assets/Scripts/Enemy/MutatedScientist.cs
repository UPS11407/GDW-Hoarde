using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedScientist : EnemyBase
{
    bool attacking = false;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject projectileStart;
    [SerializeField] float projectileSpeed;

    private void Update()
    {
        EnemyUpdate();

        if (GetPlayerDistance() <= _attackRange & !attacking)
        {
            StartCoroutine(AttackPlayer());
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
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, projectileStart.transform.position, projectileStart.transform.rotation);
            Destroy(gameObject);
        }
    }

    IEnumerator AttackPlayer()
    {
        attacking = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.6f);
        attacking = false;
        animator.SetBool("isAttacking", false);

        Shoot();
    }

    public void Shoot()
    {
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, projectileStart.transform.position, projectileStart.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = (player.transform.position - projectile.transform.position).normalized * (projectileSpeed + Random.Range(-7,0));
    }
}
