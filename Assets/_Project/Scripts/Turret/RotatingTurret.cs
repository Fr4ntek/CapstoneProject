using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTurret : MonoBehaviour
{
    [SerializeField] private Transform _target;

    void Update()
    {
        if (_target == null) return;
        transform.forward = transform.position - _target.position;
    }
}
