using System;
using System.Collections.Generic;
using UnityEngine;
public enum GemTypeEnum
{
    Red,
    Yellow,
    Blue
}
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _requiredGems = 3;

  
    private HashSet<GemTypeEnum> _collectedGems = new HashSet<GemTypeEnum>();

    // salvo tutto cio che prendo dopo l'ultimo checkpoint
    private List<GameObject> _recentPickups = new List<GameObject>();
    public IReadOnlyList<GameObject> RecentPickups => _recentPickups;

    public int Coins { get; set; } = 0;
    public int Health { get; private set; }
    public IReadOnlyCollection<GemTypeEnum> CollectedGems => _collectedGems;

    public event Action<int> OnCoinCollected;
    public event Action<GemTypeEnum> OnGemCollected;
    public event Action OnGemsReset;
    public event Action OnAllGemsCollected;
    private LifeController _lifeController;

    private void OnEnable()
    {
        _lifeController = GetComponent<LifeController>();
        _lifeController.OnLifeChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        _lifeController.OnLifeChanged -= UpdateHealth;
    }

    private void UpdateHealth(int hp, int maxHp)
    {
        Health = hp;
    }

    public void AddCoin()
    {
        GameSession.Instance.AddCoin(1);
        Coins += 1;
        // Lancio evento per aggiornare UI
        OnCoinCollected(Coins);
    }

    public void RestoreCoinsCheckpoint(int coins)
    {
        Coins = coins;
        OnCoinCollected(Coins);
    }

    public void CollectGem(GemTypeEnum gemType)
    {
        if (_collectedGems.Add(gemType))
        {
            // Lancio evento per aggiornare UI
            OnGemCollected(gemType);
        }
        if (_collectedGems.Count == _requiredGems)
        {
            // Lancio evento per aprire la porta finale
            OnAllGemsCollected();
        } 
    }

    public void RestoreGemsCheckpoint(HashSet<GemTypeEnum> restoredGems)
    {
        _collectedGems = restoredGems;
        OnGemsReset();
        foreach (var gem in _collectedGems)
        {
            OnGemCollected(gem);
        }
    }

    public void RegisterPickup(GameObject pickup)
    {
        _recentPickups.Add(pickup);
    }

    public void ClearRecentPickups()
    {
        _recentPickups.Clear();
    }
}
