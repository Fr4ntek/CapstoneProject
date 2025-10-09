using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{

    [SerializeField] private float _bounceForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.collider.CompareTag("Player"))
        {
            Rigidbody rb = collision.collider.attachedRigidbody;
            if (rb != null)
            {
                Vector3 velocity = rb.velocity;
                velocity.y = _bounceForce;
                rb.velocity = velocity;
            }
        }
    }
}
