using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseAI : MonoBehaviour
{
    public enum AIState
    {
        Patrolling,
        Chasing,
        Alerted,
        ReturningToPost,
        Searching
    }

    [Header("View Settings")]
    [SerializeField] protected Transform _eyePoint;
    [SerializeField] protected Transform _player;
    [SerializeField] protected float _viewAngle = 45f;
    [SerializeField] protected float _sightDistance = 15f;
    [SerializeField] protected LayerMask _whatIsObstacle;
    [SerializeField] protected int _subdivisions = 12;
    protected LineRenderer _lineRenderer;

    [Header("Patrol Settings")]
    [SerializeField] protected Transform[] _waypoints;
    [SerializeField] protected AIState _currentState = AIState.Patrolling;
    protected Vector3 _startPosition;
    protected int _currentWayPointIndex = 0;

    [Header("Search Settings")]
    [SerializeField] protected float _searchDuration = 3f;
    protected float _searchTimer = 0f;

    [Header("Damage Settings")]
    [SerializeField] protected float _attackCooldown = 1.5f;
    [SerializeField] protected int _damage = 10;
    protected float _lastAttackTime;

    protected NavMeshAgent _agent;

    protected virtual void Start()
    {
        _startPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        if (_lineRenderer != null) ChangeLineColor(Color.white);
       
        EvaluateConeOfView(_subdivisions);
        ChangeState(AIState.Patrolling);
    }

    protected virtual void Update()
    {
        switch (_currentState)
        {
            case AIState.Patrolling:
                PatrolState();
                break;
            case AIState.Chasing:
                ChaseState();
                break;
            case AIState.Alerted:
                AlertState();
                break;
            case AIState.Searching:
                SearchState();
                break;
            case AIState.ReturningToPost:
                ReturnToPostState();
                break;
        }

        if (_currentState != AIState.Chasing && CanSeePlayer())
        {
            ChangeState(AIState.Chasing);
        }
    }

    protected virtual void ChangeState(AIState newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case AIState.Patrolling:
                PerformPatrol();
                break;
            case AIState.Chasing:
            case AIState.Alerted:
                _agent.isStopped = false;
                ChangeLineColor(Color.red);
                break;
            case AIState.Searching:
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
                break;
            case AIState.ReturningToPost:
                _agent.isStopped = false;
                _agent.SetDestination(_startPosition);
                break;
        }
    }

    // STATES SECTION

    // Idle state
    protected virtual void PatrolState()
    {
        PerformPatrol(); 
    }

    protected void PerformPatrol()
    {
        if (_waypoints.Length == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Length;
            _agent.SetDestination(_waypoints[_currentWayPointIndex].position);
        }
    }

    // Chase state
    protected virtual void ChaseState()
    {
        if (_player == null) return;

        _agent.SetDestination(_player.position);

        if (!CanSeePlayer())
        {
            ChangeState(AIState.Searching);
        }
    }

    // Alerted state
    protected virtual void AlertState() {}

    // Search state
    protected virtual void SearchState() 
    {
        // Rimane fermo qualche secondo poi torna al punto iniziale
        ChangeLineColor(Color.white);
        _searchTimer += Time.deltaTime;

        if (CanSeePlayer())
        {
            ChangeState(AIState.Chasing);
            return;
        }

        if (_searchTimer >= _searchDuration)
        {
            _searchTimer = 0f;
            ChangeState(AIState.ReturningToPost);
        }
    }
        
    // ReturnToPost state
    protected virtual void ReturnToPostState()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
        {
            ChangeState(AIState.Patrolling);
        }
    }

    // SIGHT SECTION
    protected bool CanSeePlayer()
    {
        if (_player == null) return false;

        Vector3 toPlayer = _player.position - transform.position;
        float sqrDist = toPlayer.sqrMagnitude;

        if (sqrDist > _sightDistance * _sightDistance)
            return false;

        if (Vector3.Dot(transform.forward, toPlayer.normalized) < Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
            return false;

        if (Physics.Linecast(_eyePoint.position, _player.position, _whatIsObstacle))
            return false;

        return true;
    }

    public void EvaluateConeOfView(int subdivisions)
    {
        float startAngle = (90 - _viewAngle) * Mathf.Deg2Rad;

        int totalPoints = subdivisions + 1;
        _lineRenderer.positionCount = totalPoints;

        Vector3[] arcPoints = new Vector3[totalPoints];

        float stepAngle = (2 * _viewAngle / subdivisions) * Mathf.Deg2Rad;

        for (int i = 0; i < subdivisions; i++)
        {
            float currentAngle = startAngle + i * stepAngle;

            arcPoints[i].x = Mathf.Cos(currentAngle) * _sightDistance;
            arcPoints[i].z = Mathf.Sin(currentAngle) * _sightDistance;
        }

        arcPoints[subdivisions] = Vector3.zero;

        _lineRenderer.SetPositions(arcPoints);
    }

    private void ChangeLineColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

}
