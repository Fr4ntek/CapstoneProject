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

    private HashSet<int> _collectedCoinIDs = new HashSet<int>();
    private HashSet<int> _checkpointCollectedCoinIDs = new HashSet<int>();
    private HashSet<GemTypeEnum> _collectedGems = new HashSet<GemTypeEnum>();

    public int Coins { get; private set; } = 0;
    public int Health { get; private set; }
    public IReadOnlyCollection<GemTypeEnum> CollectedGems => _collectedGems;
    public bool CollectedCoinsContains(int coinID) => _collectedCoinIDs.Contains(coinID);

    public event Action<int> OnCoinCollected;
    public event Action<GemTypeEnum> OnGemCollected;
    public event Action OnAllGemsCollected;
    private CoinPicker[] _allCoins;
    private LifeController _lifeController;

    private void Awake()
    {
        _allCoins = FindObjectsOfType<CoinPicker>();
    }

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

    public void AddCoin(int coinID)
    {
        if (_collectedCoinIDs.Add(coinID))
        {
            Coins += 1;
            // Lancio evento per aggiornare UI
            OnCoinCollected(Coins);
        }
    }

    public void SaveCoinsCheckpoint()
    {
        _checkpointCollectedCoinIDs = new HashSet<int>(_collectedCoinIDs);
    }

    public void LoadCoinsCheckpoint()
    {
        _collectedCoinIDs = new HashSet<int>(_checkpointCollectedCoinIDs);
        Coins = _collectedCoinIDs.Count;
        OnCoinCollected(Coins);
        ResetCoinsCheckpoint();
    }

    public void ResetCoinsCheckpoint()
    {
        foreach (var coin in _allCoins)
        {
            coin.ResetCoin(this);
        }
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
}
