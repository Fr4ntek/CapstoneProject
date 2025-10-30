using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _scoreTexts;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Transform _showPoint;
    [SerializeField] private Transform _hidePoint;
    [SerializeField] private float _animationDuration = 0.5f;


    private bool _isVisible = false;

    private void OnEnable()
    {
        UpdateLeaderboard();
    }

    public void ToggleLeaderboard()
    {
        if (!_isVisible)
            _panel.DOAnchorPos(_showPoint.localPosition, _animationDuration).SetEase(Ease.OutCubic);
        else
            _panel.DOAnchorPos(_hidePoint.localPosition, _animationDuration).SetEase(Ease.InCubic);

        _isVisible = !_isVisible;
    }

    private void UpdateLeaderboard()
    {
        List<LeaderboardManager.Entry> scores = LeaderboardManager.LoadScores();

        for (int i = 0; i < _scoreTexts.Length; i++)
        {
            if (i < scores.Count)
                _scoreTexts[i].text = $"{i + 1}. {scores[i].playerName} — {scores[i].score}";
            else
                _scoreTexts[i].text = $"{i + 1}. ---";
        }

        //if (_currentScoreText != null && ScoreManager.Instance != null)
        //{
        //    _currentScoreText.text = $"Punteggio: {ScoreManager.Instance.GetCurrentScore()}";
        //}
    }

    public void ResetLeaderboard()
    {
        LeaderboardManager.ResetScore();
        UpdateLeaderboard();
    }
}
