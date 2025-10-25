using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CoinPicker : MonoBehaviour
{
    [SerializeField] private int _coinID = 0;
    [SerializeField] private float _rotateSpeed = 90f;

    // contatore globale per utilizzo checkpoint
    private static int _nextID = 0;

    private void Awake()
    {
        if (_coinID == 0)
        {
            _coinID = ++_nextID;
        }
    }
    private void OnEnable()
    {
        transform.DORotate(
            transform.localRotation.eulerAngles + new Vector3(0, 360, 0),
            360f / _rotateSpeed,
            RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Play("Coin");
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
                stats.AddCoin(_coinID);

            DOTween.Kill(transform);
            gameObject.SetActive(false);
        }
    }

    public void ResetCoin(PlayerStats stats)
    {
        if (!stats.CollectedCoinsContains(_coinID))
            gameObject.SetActive(true);
    }
}
