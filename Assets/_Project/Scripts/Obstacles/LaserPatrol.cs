using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaserPatrol : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _duration = 5f;
    void Start()
    {
        if (_waypoints.Length == 0) return;

        Vector3[] path = new Vector3[_waypoints.Length + 1];
        for (int i = 0; i < _waypoints.Length; i++)
            path[i] = _waypoints[i].position;

        path[_waypoints.Length] = _waypoints[0].position;

        transform.DOPath(path, _duration, PathType.Linear, PathMode.Full3D)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
    }
}

