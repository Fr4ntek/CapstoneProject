using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpikeAI : EnemyBaseAI
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
    }
    // fa tutto ciò che fa la base
    // nel caso posso aggiungere qui comportamenti extra
}
