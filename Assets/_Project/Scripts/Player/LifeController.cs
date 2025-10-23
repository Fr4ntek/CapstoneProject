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
    
    public UnityEvent<int, int> onLifeChanged;
    private bool _isDead = false;
    private UIController _uiController;

    private void Awake()
    {
        if (_fullHpOnStart) _hp = _maxHp;
    }

    void Start()
    {
        onLifeChanged?.Invoke(_hp, _maxHp); 
        _uiController = GetComponent<UIController>();
    }

    public void SetHp(int amount)
    {
        _hp = Mathf.Clamp(amount, 0, _maxHp);
        onLifeChanged?.Invoke(_hp, _maxHp);
        if (_hp <= 0) Die();
    }

    public void AddHp(int amount)
    {
        SetHp(_hp + amount);
    }

    public void TakeDamage(int damage)
    {
        AddHp(-damage);
    }

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        if (_uiController != null) _uiController.ShowDeathUI();
        Destroy(gameObject);
    }
}
