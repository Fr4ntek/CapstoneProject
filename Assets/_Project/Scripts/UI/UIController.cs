using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("HealthBar/Coin")]
    [SerializeField] private Image _healthBarSprite;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private TextMeshProUGUI _coinCounterText;
    [SerializeField] private GameObject _coinWarningMessage;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _countdownTime = 60f;
    
    [Header("Gems")]
    [SerializeField] private Image _redGemUI;
    [SerializeField] private Image _yellowGemUI;
    [SerializeField] private Image _blueGemUI;

    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Image _fadeOverlay;

    [Header("Fade Settings")]
    [SerializeField] private float _fadeDuration = 0.5f;

    public int _coinCount = 0;
    private float _timeLeft;
    private bool _isTimeRunning = false;
    private LifeController _lifeController;
    private PlayerStats _playerStats;


    private void OnEnable()
    {
        _playerStats = GetComponent<PlayerStats>();
        _lifeController = GetComponent<LifeController>();
        _playerStats.OnCoinCollected += UpdateCoinUI;
        _playerStats.OnGemCollected += UpdateGemUI;
        _lifeController.OnLifeChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        _playerStats.OnCoinCollected -= UpdateCoinUI;
        _playerStats.OnGemCollected -= UpdateGemUI;
        _lifeController.OnLifeChanged -= UpdateHealthBar;
    }

    void Start()
    {   
        UpdateCoinUI(0);
        SetTimerUI();
    }

    private void SetTimerUI()
    {
        _timeLeft = _countdownTime;
        _isTimeRunning = true;
    }

    void Update()
    {
        UpdateTimerUI();
        if (_timeLeft <= 0f)
        {
            _isTimeRunning = false;
            _lifeController.Die();
        }
    }

    private void UpdateTimerUI()
    {
        if (!_isTimeRunning) return;
        _timeLeft -= Time.deltaTime;
        _timeLeft = Mathf.Max(0f, _timeLeft);

        int minutes = Mathf.FloorToInt(_timeLeft / 60f);
        int seconds = Mathf.FloorToInt(_timeLeft % 60f);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateGemUI(GemTypeEnum gemType)
    {
        switch (gemType)
        {
            case GemTypeEnum.Red:
                _redGemUI.color = Color.white;
                break;
            case GemTypeEnum.Yellow:
                _yellowGemUI.color = Color.white;
                break;
            case GemTypeEnum.Blue:
                _blueGemUI.color = Color.white;
                break;
        }
    }

    private void UpdateCoinUI(int coins)
    {
        _coinCounterText.text = coins.ToString();
    }

    public void UpdateHealthBar(int hp, int maxHp)
    {
        _healthBarSprite.fillAmount = (float) hp / maxHp;
        _healthBarSprite.color = _gradient.Evaluate(_healthBarSprite.fillAmount);
    }

    public void ShowVictoryUI()
    {
        _gameManager.ShowVictoryUI();
    }

    public void ShowDeathUI()
    {
        _gameManager.ShowDeathUI();
    }

    public IEnumerator FadeIn()
    {
        yield return _fadeOverlay.DOFade(1f, _fadeDuration).WaitForCompletion();
    }

    public IEnumerator FadeOut()
    {
        yield return _fadeOverlay.DOFade(0f, _fadeDuration).WaitForCompletion();
    }

    //public void RefreshGemUI()
    //{
    //    redGemUI.color = playerStats.HasGem(GemType.Red) ? Color.white : Color.gray;
    //    blueGemUI.color = playerStats.HasGem(GemType.Blue) ? Color.white : Color.gray;
    //    greenGemUI.color = playerStats.HasGem(GemType.Green) ? Color.white : Color.gray;
    //}
}
