using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    public int collectedCoins = 0;
    public float timeLeft = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        collectedCoins += amount;
    }

    public void ResetCoins()
    {
        collectedCoins = 0;
    }
}
