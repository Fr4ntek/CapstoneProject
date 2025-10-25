using DG.Tweening;
using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _rotateSpeed = 360f;
    [SerializeField] private int _damage = 20;

    private void Start()
    {
        transform.DOMove(_endPosition.position, _duration)
         .SetEase(Ease.InOutSine)
         .SetLoops(-1, LoopType.Yoyo);

        transform.DORotate(
            transform.localRotation.eulerAngles + new Vector3(0, 0, 360),
            360f / _rotateSpeed,
            RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
    }
}
