using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DeathPoint : MonoBehaviour
{
    [SerializeField] private UIController _uiController;
    [SerializeField] private CheckpointManager _checkpointManager;

    private void OnTriggerEnter(Collider other)
    { 
        if (!other.CompareTag("Player")) return;
        _uiController = other.GetComponent<UIController>();
        _checkpointManager =GetComponent<CheckpointManager>();
        RespawnOrDeath(other.gameObject);
    }
    private void RespawnOrDeath(GameObject player)
    {
        if (SaveSystem.HasCheckpoint())
        {
            StartCoroutine(RestoreCheckpointRoutine(player));
        }
        else
        {
            _uiController.ShowDeathUI();
        }
    }
    private IEnumerator RestoreCheckpointRoutine(GameObject player)
    {
        yield return _uiController.FadeIn();

        _checkpointManager.RestoreCheckpoint(player);

        yield return _uiController.FadeOut();
    }
}




