using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public void RestoreCheckpoint(GameObject player)
    {
        CheckpointData data = SaveSystem.LoadCheckpoint();
        if (data == null) return;
        // Ripristina posizione
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = data.playerPosition;
        cc.enabled = true;

        // Ripristina stats
        PlayerStats stats = player.GetComponent<PlayerStats>();
        LifeController lc = player.GetComponent<LifeController>();
        lc.SetHp(data.hp);
        stats.LoadCoinsCheckpoint();
        //stats.RestoreGems(data.collectedGems);

        // Puoi anche aggiornare timer e UI se serve
    }
}
