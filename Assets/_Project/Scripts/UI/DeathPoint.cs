using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoint: MonoBehaviour
{
    [SerializeField] GameObject canvas;

    private void OnTriggerEnter(Collider other)
    {
        LifeController lc = other.GetComponent<LifeController>();
        if (lc != null) lc.Die();
    }
}
