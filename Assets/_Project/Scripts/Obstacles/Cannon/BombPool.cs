using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private int _poolSize = 10;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_bombPrefab, transform);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject GetBomb()
    {
        if (_pool.Count > 0)
        {
            GameObject bomb = _pool.Dequeue();
            bomb.SetActive(true);
            return bomb;
        }
        else
        {
            // Se il pool è esaurito puoi istanziare nuovo
            GameObject bomb = Instantiate(_bombPrefab, transform);
            return bomb;
        }
    }

    public void ReturnBomb(GameObject bomb)
    {
        bomb.SetActive(false);
        bomb.GetComponent<Renderer>().enabled = true; 
        _pool.Enqueue(bomb);
    }
}
