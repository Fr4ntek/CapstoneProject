using DG.Tweening;
using UnityEngine;

public class GemPicker : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private GemTypeEnum _gemType;

    private void Start()
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
        if (!other.CompareTag("Player")) return;
       
        AudioManager.Instance.Play("Gem");
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
            stats.CollectGem(_gemType);
        
        DOTween.Kill(transform);
        Destroy(gameObject);
    }
}

