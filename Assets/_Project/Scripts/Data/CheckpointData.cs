using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CheckpointData
{
    public string checkpointID;
    public DateTime timestamp;
    public string sceneName;
    public Vector3 playerPosition;
    public int hp;
    public float timeLeft;
    public int collectedCoins;
    public List<string> collectedGems;
}
