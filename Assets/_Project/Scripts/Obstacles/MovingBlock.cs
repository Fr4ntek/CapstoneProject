using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class MovingBlock : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private AIState _currentState;

    [Header("View Settings")]
    [SerializeField] private Transform _enemy;
    [SerializeField] private float _viewAngle = 45f;
    [SerializeField] private float _sightDistance = 15f;
    [SerializeField] private LayerMask _whatIsObstacle;
    [SerializeField] private int _subdivisions = 12;

    [Header("Damage Settings")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _damageCooldown = 1f;
    private float _lastHitTime;

    private Vector3 _startPosition;
    private int _currentWayPointIndex = 0;
    private Transform _target;
    private NavMeshAgent _agent;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _startPosition = transform.position;

        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _agent = GetComponent<NavMeshAgent>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        if (_lineRenderer != null)
        {
            _lineRenderer.startColor = Color.white;
            _lineRenderer.endColor = Color.white;
        }

        EvaluateConeOfView(_subdivisions);
        ChangeState(AIState.Idle);
    }
    void Update()
    {
        switch (_currentState)
        {
            case AIState.Idle:
                IdleState();
                break;
            case AIState.Chasing:
                ChaseState();
                break;
            case AIState.ReturningToPost:
                ReturnToPostState();
                break;
        }

        if (_currentState != AIState.Chasing && CanSeePlayer())
        {
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
            ChangeState(AIState.Chasing);
        }
    }

    private void ChangeState(AIState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case AIState.Idle:
                PerformPatrol();
                break;

            case AIState.Chasing:
                _agent.isStopped = false;
                break;

            case AIState.ReturningToPost:
                _agent.isStopped = false;
                _agent.SetDestination(_startPosition);
                break;
        }
    }

    private void IdleState()
    {
       PerformPatrol();
    }

    private void ChaseState()
    {
        _agent.SetDestination(_target.position);

        if (!CanSeePlayer())
        {
            _lineRenderer.startColor = Color.white;
            _lineRenderer.endColor = Color.white;
            ChangeState(AIState.ReturningToPost);
        }
    }

    private void ReturnToPostState()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
        {
            ChangeState(AIState.Idle);
        }
    }

    private void PerformPatrol()
    {
        if (_waypoints.Length == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Length;
            _agent.SetDestination(_waypoints[_currentWayPointIndex].position);
        }
    }

    private bool CanSeePlayer()
    {
        if(_target == null) return false;

        Vector3 toTarget = _target.position - transform.position;
        float sqrDistance = toTarget.sqrMagnitude;

        if (sqrDistance > _sightDistance * _sightDistance)
            return false;

        float distance = Mathf.Sqrt(sqrDistance);
        toTarget /= distance;

        if (Vector3.Dot(transform.forward, toTarget) < Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
            return false;

        if (Physics.Linecast(_enemy.position, _target.position, _whatIsObstacle))
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           collision.gameObject.GetComponent<LifeController>().TakeDamage(_damage); 
        }
    }
}
