using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CoinPicker : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 90f;
    
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
            stats.AddCoin();
            if (SaveSystem.HasCheckpoint())
            {
                stats.RegisterPickup(gameObject);
            }

            DOTween.Kill(transform);
            gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
