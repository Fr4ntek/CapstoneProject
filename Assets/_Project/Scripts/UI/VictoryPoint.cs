using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPoint : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    private void OnTriggerEnter(Collider other)
    {
        UIController ui = other.GetComponent<UIController>();
        if (ui != null) ui.ShowVictoryUI();
    }
}
