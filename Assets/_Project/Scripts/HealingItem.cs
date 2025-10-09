using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [SerializeField] private int _healingAmount = 30;
    void Update()
    {
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LifeController>().AddHp(_healingAmount);
            Destroy(gameObject);
        }
    }
}
