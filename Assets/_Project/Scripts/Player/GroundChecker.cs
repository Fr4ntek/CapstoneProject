using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _radius = 0.1f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private UnityEvent<bool> _onIsGroundedChanged;

    public bool IsGrounded { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _radius);
    }

    void Update()
    {
        // salvo la variabile per far si che invoco l'evento solo quando cambia
        bool wasGrounded = IsGrounded; 

        IsGrounded = Physics.CheckSphere(transform.position, _radius, _whatIsGround);
        
        if(wasGrounded != IsGrounded)
        {
            _onIsGroundedChanged?.Invoke(IsGrounded);
        }
    }
}
