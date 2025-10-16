using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.AI;

public class GuardPatrol : MonoBehaviour
{
    public Transform[] _patrolPoints;
    private NavMeshAgent _agent;
    private int _currentIndex;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_patrolPoints.Length > 0)
        {
            _agent.destination = _patrolPoints[0].position;
        }
    }

    void Update()
    {
        if (_patrolPoints.Length == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance < 0.3f)
        {
            _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
            _agent.destination = _patrolPoints[_currentIndex].position;
        }
    }
}

