using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyGuardAI : EnemyBaseAI
{
    [Header("NavMesh Speeds")]
    [SerializeField] private float _patrolSpeed = 2.4f;
    [SerializeField] private float _chaseSpeed = 5f;

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
            case AIState.Chasing:
            case AIState.Alerted:
                if (!AudioManager.Instance.IsPlaying("ChaseMusic"))
                {
                    AudioManager.Instance.Play("ChaseMusic");
                }
                _agent.speed = _chaseSpeed;
                break;
            case AIState.Patrolling:
            case AIState.ReturningToPost:
                _agent.speed = _patrolSpeed;
                break;
            case AIState.Searching:
                if (AudioManager.Instance.IsPlaying("ChaseMusic"))
                {
                    AudioManager.Instance.Stop("ChaseMusic");
                }
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

    public bool IsAlerted()
    {
        return _currentState == AIState.Alerted;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<LifeController>().RespawnOrDie(false);
        }
    }
}
