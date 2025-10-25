using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpikeAI : EnemyBaseAI
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Time.time -_lastAttackTime >= _attackCooldown)
            {
                collision.GetComponent<LifeController>().TakeDamage(_damage);
                _lastAttackTime = Time.time;
            }
            
        }
    }
}
