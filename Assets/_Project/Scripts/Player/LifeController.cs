using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _hp;
    [SerializeField] private bool _fullHpOnStart = true;
    //[SerializeField] private GameManager _gameManager;

    public event Action<int, int> OnLifeChanged;

    private bool _isDead = false;
    private UIController _uiController;
    private CheckpointManager _checkpointManager;

    private void Awake()
    {
        if (_fullHpOnStart) _hp = _maxHp;
    }

    void Start()
    {
        OnLifeChanged(_hp, _maxHp);
        _uiController = GetComponent<UIController>();
        _checkpointManager = GetComponent<CheckpointManager>();
    }

    public void SetHp(int amount)
    {
        _hp = Mathf.Clamp(amount, 0, _maxHp);
        OnLifeChanged(_hp, _maxHp);

        if (_hp <= 0) RespawnOrDie(false);
    }

    public void AddHp(int amount)
    {
        SetHp(_hp + amount);
    }

    public void TakeDamage(int damage)
    {
        GetComponentInChildren<PlayerDamageFeedback>().FlashDamage();
        AddHp(-damage);
    }

    public void RespawnOrDie(bool timerExpired)
    {
        if (_isDead) return;

        if (!timerExpired && SaveSystem.HasCheckpoint())
        {
            _isDead = true;
            StartCoroutine(RestoreCheckpointRoutine(gameObject));
        }
        else
        {
            _isDead = true;
            _uiController.ShowDeathUI();
        }
    }

    private IEnumerator RestoreCheckpointRoutine(GameObject player)
    {
        yield return _uiController.FadeIn();

        _checkpointManager.RestoreCheckpoint(player);
        _isDead = false;

        yield return _uiController.FadeOut();
    }
}