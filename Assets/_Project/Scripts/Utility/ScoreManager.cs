using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int _currentScore;
    private string _playerName = "Player";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public string GetPlayerName() => _playerName;
    public void SetPlayerName(string name)
    {
        _playerName = string.IsNullOrEmpty(name) ? "Player" : name;
    }

    public int GetCurrentScore() => _currentScore;
    public int CalculateScore(int coins, int gems, float elapsedTime, int hp, int extraPoints)
    {
        _currentScore = coins * 50 + gems * 500 + hp * 5 - Mathf.RoundToInt(elapsedTime * 2) + extraPoints;
        if (_currentScore < 0) _currentScore = 0;
        return _currentScore;
    }

    public void SaveScore()
    {
        LeaderboardManager.SaveNewScore(_playerName, _currentScore);
    }
}
