using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 90;

    void Update()
    {
        transform.parent.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody rb = collision.collider.attachedRigidbody;

            // Sposta il player con la piattaforma (mettendolo come figlio della piattaforma perdevo il movimento del player)
            // Pos del player rispetto al centro della piattaforma
            Vector3 dir = rb.position - transform.position;
            dir.y = 0;
            // Ruota un pò il player
            Vector3 rotatedDir = Quaternion.Euler(0, _rotationSpeed * Time.fixedDeltaTime, 0) * dir;
            // Calcola spostamento del player per seguire la piccola rotazione
            Vector3 move = rotatedDir - dir;
            rb.MovePosition(rb.position + move);
        }
    }
}
