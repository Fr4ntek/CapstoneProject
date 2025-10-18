using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyGuardAI : EnemyBaseAI
{
    [Header("NavMesh Speeds")]
    public float _patrolSpeed = 2.4f;
    public float _chaseSpeed = 5f;

    private Animator _animator;

    protected override void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        base.Start();
        _damage = 100;
    }

    protected override void ChangeState(AIState newState)
    {
        base.ChangeState(newState);
        switch (newState)
        {
            case AIState.Patrolling:
                _agent.speed = _patrolSpeed;
                break;
            case AIState.Chasing:
            case AIState.Alerted:
                _agent.speed = _chaseSpeed;
                break;
            case AIState.ReturningToPost:
                _agent.speed = _patrolSpeed;
                break;
        }
        UpdateAnimator();
    }

    protected override void AlertState()
    {
        // Se vedono il player passo a Chasing
        if (CanSeePlayer())
        {
            ChangeState(AIState.Chasing);
            return;
        }

        // Altrimenti continuano verso l'ultima posizione conosciuta 
        if (_agent.pathPending) return;                      
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            ChangeState(AIState.Searching);
            return;
        }
    }

    protected override void SearchState()
    {
        _searchDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
        base.SearchState();
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("IsWalking", _currentState == AIState.Patrolling || _currentState == AIState.ReturningToPost);
        _animator.SetBool("IsChasing", _currentState == AIState.Chasing || _currentState == AIState.Alerted);
        _animator.SetBool("IsSearching", _currentState == AIState.Searching);
    }

    public void ChasePosition(Vector3 playerPos)
    { 
        _agent.SetDestination(playerPos);
        ChangeState(AIState.Alerted);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
    }
}
