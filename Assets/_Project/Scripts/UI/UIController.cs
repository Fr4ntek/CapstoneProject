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

    [Header("Timer/Score")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _countdownTime = 60f;
    [SerializeField] private TextMeshProUGUI _winScoreText;
    [SerializeField] private TextMeshProUGUI _deathScoreText;
    private int _finalScore = 0;

    [Header("Gems")]
    [SerializeField] private Image _redGemUI;
    [SerializeField] private Image _yellowGemUI;
    [SerializeField] private Image _blueGemUI;

    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Image _fadeOverlay;
    [SerializeField] private GameObject _deathUI;
    [SerializeField] private GameObject _victoryUI;
    [SerializeField] private GameObject _pauseUI;

    [Header("Fade Settings")]
    [SerializeField] private float _fadeDuration = 0.25f;

    public int _coinCount = 0;
    private bool _canPause = true;
    private bool _isTimeRunning = false;
    private LifeController _lifeController;
    private PlayerStats _playerStats;
    private bool _isPaused;

    private void OnEnable()
    {
        _playerStats = GetComponent<PlayerStats>();
        _lifeController = GetComponent<LifeController>();
        _playerStats.OnCoinCollected += UpdateCoinUI;
        _playerStats.OnGemCollected += UpdateGemUI;
        _playerStats.OnGemsReset += ResetGemUI;
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
    void Update()
    {
        UpdateTimerUI();
        if (GameSession.Instance.timeLeft <= 0f)
        {
            _isTimeRunning = false;
            _lifeController.RespawnOrDie(true);
        }

        if (_canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseUI();
        }
    }

    public void TogglePauseUI()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
        AudioListener.pause = _isPaused;
        _pauseUI.SetActive(_isPaused);
        if (_isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void Resume()
    {
        _pauseUI.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
        AudioListener.pause = _isPaused;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetTimerUI()
    {
        _countdownTime = GameSession.Instance.timeLeft;
        _isTimeRunning = true;
    }

    private void UpdateTimerUI()
    {
        if (!_isTimeRunning) return;
        GameSession.Instance.timeLeft -= Time.deltaTime;
        GameSession.Instance.timeLeft = Mathf.Max(0f, GameSession.Instance.timeLeft);

        int minutes = Mathf.FloorToInt(GameSession.Instance.timeLeft / 60f);
        int seconds = Mathf.FloorToInt(GameSession.Instance.timeLeft % 60f);

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
    public void ResetGemUI()
    {
        if( _redGemUI != null && _blueGemUI != null && _redGemUI != null)
        {
            _blueGemUI.color = Color.gray;
            _yellowGemUI.color = Color.gray;
            _redGemUI.color = Color.gray;
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

    public void ShowVictoryUI(int extraPoints)
    {
        GameOver(extraPoints);
        AudioManager.Instance.Play("Level2Completed");
        _victoryUI.SetActive(true);
        _winScoreText.text = "Score: " + _finalScore;
    }

    public void ShowDeathUI()
    {
        GameOver(0);
        AudioManager.Instance.Play("GameOver"); 
        _deathUI.SetActive(true);
        _deathScoreText.text = "Score: " + _finalScore;
    }

    private void GameOver(int extraPoints)
    {
        Time.timeScale = 0f;
        _canPause = false;
        AudioManager.Instance.StopAll();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int coins = GameSession.Instance.collectedCoins;
        int gems = _playerStats.CollectedGems.Count;
        float elapsedTime = _countdownTime - GameSession.Instance.timeLeft;
        int hp = _playerStats.Health;
        Debug.Log($"Coins: {coins} - Gems: {gems} - ElapsedTime: {elapsedTime} - HP: {hp}");

        _finalScore += ScoreManager.Instance.CalculateScore(coins, gems, elapsedTime, hp, extraPoints);
        ScoreManager.Instance.SaveScore();
        SaveSystem.ClearCheckpoint();
        GameSession.Instance.ResetSession();
    }

    public IEnumerator FadeIn()
    {
        yield return _fadeOverlay.DOFade(1f, _fadeDuration).WaitForCompletion();
    }

    public IEnumerator FadeOut()
    {
        yield return _fadeOverlay.DOFade(0f, _fadeDuration).WaitForCompletion();
    }

}
