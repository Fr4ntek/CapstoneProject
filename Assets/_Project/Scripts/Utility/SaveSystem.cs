using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "saveData.json");

    public static void SaveCheckpoint(CheckpointData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Checkpoint saved: {data.checkpointID} at {data.playerPosition}");
    }

    public static CheckpointData LoadCheckpoint()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No checkpoint file found.");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        CheckpointData data = JsonUtility.FromJson<CheckpointData>(json);
        Debug.Log($"Checkpoint loaded: {data.checkpointID} from {data.sceneName}");
        return data;
    }

    public static void ClearCheckpoint()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    public static bool HasCheckpoint()
    {
        return File.Exists(SavePath);
    }
  
}
