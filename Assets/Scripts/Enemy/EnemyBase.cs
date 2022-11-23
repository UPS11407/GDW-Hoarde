using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    public float _maxHP = 4.0f;
    public float _speed = 2.0f;
    public float _damage = 1.0f;
    public float _attackRange = 2.0f;
    public GameObject healPickupPrefab;

    [Tooltip("% chance to drop a heal pickup")]
    [Range(0, 100)]
    public int healDropChance;
    float currentHP;

    int dropVal;

    internal GameObject player;

    Rigidbody rigid;
    NavMeshAgent agent;

    private void Awake()
    {
        player = GameObject.Find("Player");
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = _speed;

        currentHP = _maxHP;
    }

    public void EnemyUpdate()
    {
        CheckIfDead();
        ChasePlayer();
    }

    public void CheckIfDead()
    {
        if(currentHP <= 0)
        {
            dropVal = Random.Range(0, 100);

            if(dropVal < healDropChance)
            {
                DropPickup();
            }

            Destroy(gameObject);
        }
    }

    void DropPickup()
    {
        Instantiate(healPickupPrefab, transform.position, Quaternion.Euler(0, 0, 0));
    }

    /// <summary>
    /// Returns the distance between the object and the player
    /// </summary>
    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    /// <summary>
    /// Sets the Nav Mesh Agent's target to the player's current position.
    /// Also stops agent's movement if they are in attack range
    /// </summary>
    public void ChasePlayer()
    {
        if(GetPlayerDistance() <= _attackRange)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        if(agent.destination != player.transform.position)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public void TakeDamage(float val)
    {
        currentHP -= val;
    }
}
