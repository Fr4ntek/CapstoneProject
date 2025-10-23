using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("HealthBar")]
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

    [SerializeField] private GameManager _gameManager;

    public int _coinCount = 0;
    private float _timeLeft;
    private bool _isTimeRunning = false;
    private LifeController _lifeController;

    void Start()
    {
        _lifeController = GetComponent<LifeController>();
        UpdateCoinUI();
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
    }

    private void UpdateTimerUI()
    {
        if (!_isTimeRunning) return;
        _timeLeft -= Time.deltaTime;
        _timeLeft = Mathf.Max(0f, _timeLeft);

        int minutes = Mathf.FloorToInt(_timeLeft / 60f);
        int seconds = Mathf.FloorToInt(_timeLeft % 60f);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (_timeLeft <= 0f)
        {
            _isTimeRunning = false;
            _lifeController.Die();
        }
    }

    public void AddCoin(int amount)
    {
        _coinCount += amount;
        AudioManager.Instance.Play("Coin");
        UpdateCoinUI();
    }

    public void UpdateGemUI(GemPicker.GemType color)
    {
        switch (color)
        {
            case GemPicker.GemType.Red:
                _redGemUI.color = Color.white;
                break;
            case GemPicker.GemType.Yellow:
                _yellowGemUI.color = Color.white;
                break;
            case GemPicker.GemType.Blue:
                _blueGemUI.color = Color.white;
                break;
        }
        AudioManager.Instance.Play("Gem");
    }

    private void UpdateCoinUI()
    {
        _coinCounterText.text = _coinCount.ToString();
    }

    public void UpdateHealthBar(int hp, int maxHp)
    {
        _healthBarSprite.fillAmount = (float)hp / maxHp;
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

}
