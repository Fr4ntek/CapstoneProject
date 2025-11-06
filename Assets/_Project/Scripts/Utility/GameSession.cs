using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    public int collectedCoins = 0;
    public float timeLeft = 0;
    public float defaultTime = 480;

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

    public void ResetSession()
    {
        collectedCoins = 0;
        timeLeft = defaultTime;
    }
}
