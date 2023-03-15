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
    //protected float currentHP;
    public float currentHP;

    protected int dropVal;

    internal GameObject player;
    internal Animator animator;
    public bool multipleAnimations = false;
    float walkNum = 0f;

    public float damageOverTime;
    [SerializeField] float damageOverTimeModifier;
    float dotTime;
    [SerializeField] float dotRate;

  
    [SerializeField] float slowDurationModifier;
    [SerializeField] float slowSpeedModifier;
    bool slowed;
    IEnumerator slowCoroutine;
    Rigidbody rigid;
    [SerializeField]
    float deathFadeOut = 2f;
    protected NavMeshAgent agent;
    protected BoxCollider hitbox;
    bool knockBacked;
    [SerializeField] float knockBackStrength;
    public bool canAttack = true;
    private void Awake()
    {
        player = GameObject.Find("Player");
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider>();
        UpdateSpeed();
        currentHP = _maxHP;

        walkNum = Random.Range(0f, 0.2f);

    }

    public void EnemyUpdate()
    {
        TakeDamageOverTime();
        

        CheckIfDead();
        //Debug.Log(NavMeshRemainingDistance(agent.path.corners));
        /*
        if (knockBacked)
        {

            agent.enabled = false;
            rigid.constraints = RigidbodyConstraints.None;
            rigid.velocity = -transform.forward * knockBackStrength;
            knockBacked = false;
            Debug.Log(rigid.velocity);
            //RagdollEnabler.EnableRagdoll();
        }
        else */
        if (rigid.velocity == Vector3.zero && canAttack)
        {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            agent.enabled = true;
            animator.enabled = true;
        }
        if (NavMeshRemainingDistance(agent.path.corners) <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void UpdateSpeed()
    {
        agent.speed = _speed;
    }

    /// <summary>
    /// Returns the total length of the navmesh path.
    /// </summary>
    float NavMeshRemainingDistance(Vector3[] points)
    {
        if (points.Length < 2) return 0;
        float distance = 0;
        for (int i = 0; i < points.Length - 1; i++)
            distance += Vector3.Distance(points[i], points[i + 1]);
        return distance;
    }

    public virtual void CheckIfDead()
    {
        if(currentHP <= 0)
        {
            dropVal = Random.Range(0, 100);

            if(dropVal < healDropChance)
            {
                DropPickup();
            }
            Debug.Log("Enemy should be dead");
            //Destroy(gameObject);
            //rigid.detectCollisions = false;
            canAttack = false;
            animator.enabled = false;
            agent.enabled = false;
            hitbox.enabled = false;
            Destroy(gameObject, 2f);
            this.enabled = false;
        }
    }
    protected void DropPickup()
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
    public virtual void ChasePlayer()
    {
        if (multipleAnimations)
            animator.SetFloat("walkType", walkNum);

        if (GetPlayerDistance() <= _attackRange)
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
        }
        else
        {
            animator.SetBool("isWalking", true);
            agent.isStopped = false;
        }

        if(agent.destination != player.transform.position)
        {
           // agent.ResetPath();  
            agent.SetDestination(player.transform.position);
        }
    }

    public void TakeDamage(float val)
    {
        currentHP -= val;
    }
    public void ApplyDamageOverTime(float DoT)
    {
        damageOverTime += (DoT * damageOverTimeModifier);
    }
    public void TakeDamageOverTime()
    {
        if (damageOverTime > 0)
        {
            if(Time.time > dotTime + dotRate)
            {
                dotTime = Time.time;
                DOTDamage(dotRate);
            }
            

        }

    } 
    public void DOTDamage(float damage)
    {
        
        if (damageOverTime < damage)
        {
            damageOverTime -= damageOverTime;
            TakeDamage(damageOverTime);
        } else
        {
            damageOverTime -= damage;
            TakeDamage(damage);
        }

    }

    public void ApplySlow(float duration)
    {
        if (!slowed)
        {
            slowCoroutine = Slow(duration * slowDurationModifier);
            StartCoroutine(slowCoroutine);
        }
        else
        {
            StopCoroutine(slowCoroutine);
            slowCoroutine = Slow(duration * slowDurationModifier);
            StartCoroutine(slowCoroutine);
        }


    }

    IEnumerator Slow(float slowDuration)
    {
        slowed = true;
        Debug.Log("Slowed");
        agent.speed = _speed * slowSpeedModifier;
        
        yield return new WaitForSeconds(slowDuration);
        
        slowed = false;
        agent.speed = _speed;
    }
    /*
    public void Knockback()
    {
        Debug.Log("PUNCHED");
        knockBacked = true;
        StartCoroutine(KnockbackLimiter(2));
    }*/
    public void Knockback(GameObject knockbacker)
    {
        agent.enabled = false;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.velocity = (transform.position - knockbacker.transform.position).normalized * knockBackStrength;
        KnockbackLimiter(2);
    }
    IEnumerator KnockbackLimiter(float duration)
    {
        yield return new WaitForSeconds(duration);
        rigid.velocity = Vector3.zero;
    }

}
