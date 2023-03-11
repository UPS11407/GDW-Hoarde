using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollEnabler : MonoBehaviour
{
    public BoxCollider hitbox;
    [SerializeField]
    Animator Animator;
    [SerializeField]
    Transform RagdollRoot;
    [SerializeField]
    NavMeshAgent Agent;
    [SerializeField]
    bool StartRagdoll = false;
    [SerializeField]
    EnemyBase enemyBase;
    Rigidbody[] Rigidbodies;
    CharacterJoint[] CharacterJoints;
    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        CharacterJoints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (StartRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            //EnableAnimator();
           // enemyBase.enabled = true;
        }
    }

    public void EnableRagdoll()
    {
        Animator.enabled = false;
        Agent.enabled = false;
        hitbox.enabled = false;
        enemyBase.enabled = false;
        foreach (CharacterJoint joint in CharacterJoints)
        {
            joint.enableCollision = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = true;
        }
    }

    public void DisableAllRigidbodies()
    {
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }

    public void EnableAnimator()
    {
        Animator.enabled = true;
        Agent.enabled = true;
        hitbox.enabled = true;
        foreach (CharacterJoint joint in CharacterJoints)
        {
            joint.enableCollision = false;
        }
        DisableAllRigidbodies();
    }
}