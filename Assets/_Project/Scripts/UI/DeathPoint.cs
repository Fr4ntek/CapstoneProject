using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DeathPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    { 
        if (!other.CompareTag("Player")) return;
        other.gameObject.GetComponent<LifeController>().RespawnOrDie(false);
    }
  
}




