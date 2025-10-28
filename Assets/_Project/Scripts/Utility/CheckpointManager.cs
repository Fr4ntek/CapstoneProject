using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (var pickup in stats.RecentPickups)
        {
            pickup.SetActive(true);
        }
        stats.RestoreCoinsCheckpoint(data.collectedCoins);
        stats.RestoreGemsCheckpoint(data.collectedGems
                                    .Select(g => Enum.Parse<GemTypeEnum>(g))
                                       .ToHashSet());
    }
}
