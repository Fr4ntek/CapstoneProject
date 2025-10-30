using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class LeaderboardManager
{
    private const int MaxScores = 10;
    private const string PrefsKey = "LeaderboardV2";

    [System.Serializable]
    public class Entry
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    private class EntryList
    {
        public List<Entry> entries = new List<Entry>();
    }

    public static List<Entry> LoadScores()
    {
        string json = PlayerPrefs.GetString(PrefsKey, "");
        if (string.IsNullOrEmpty(json))
            return new List<Entry>();

        return JsonUtility.FromJson<EntryList>(json).entries;
    }

    public static void SaveNewScore(string playerName, int newScore)
    {
        var scores = LoadScores();
        scores.Add(new Entry { playerName = playerName, score = newScore });

        scores = scores.OrderByDescending(e => e.score).Take(MaxScores).ToList();

        var entryList = new EntryList { entries = scores };
        string json = JsonUtility.ToJson(entryList);
        PlayerPrefs.SetString(PrefsKey, json);
        PlayerPrefs.Save();
    }

    public static void ResetScore()
    {
        PlayerPrefs.DeleteKey("LeaderboardV2");
        PlayerPrefs.Save();
    }
}
