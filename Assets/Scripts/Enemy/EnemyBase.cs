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
    public float chaseRange;
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
        //Debug.Log(NavMeshRemainingDistance(agent.path.corners));
        if (NavMeshRemainingDistance(agent.path.corners) <= chaseRange) ChasePlayer();
    }

    float NavMeshRemainingDistance(Vector3[] points)
    {
        if (points.Length < 2) return 0;
        float distance = 0;
        for (int i = 0; i < points.Length - 1; i++)
            distance += Vector3.Distance(points[i], points[i + 1]);
        return distance;
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
            agent.isStopped = true;
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
            agent.ResetPath();  
            agent.SetDestination(player.transform.position);
        }
    }

    public void TakeDamage(float val)
    {
        currentHP -= val;
    }
}
