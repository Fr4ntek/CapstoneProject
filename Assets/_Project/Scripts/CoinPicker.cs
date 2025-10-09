using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPicker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIController cc = other.GetComponent<UIController>();
            if (cc != null)
                cc.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
