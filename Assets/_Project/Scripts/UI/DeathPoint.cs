using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    { 
        if (!other.CompareTag("Player")) return;
        other.gameObject.GetComponent<LifeController>().RespawnOrDie(false);
    }
  
}




