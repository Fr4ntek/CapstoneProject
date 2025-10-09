using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private bool _startUp = false;

    private void Awake()
    {
        var anim = GetComponentInParent<Animator>();
        if (_startUp) anim.Play("show");
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
    }
}
