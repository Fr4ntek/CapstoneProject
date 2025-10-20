using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _duration = 2f;

    private Vector3 _lastPosition;
    private Vector3 _deltaPosition;
    public Vector3 DeltaPosition => _deltaPosition;

    void Start()
    {
        _lastPosition = transform.position;

        // Muove avanti e indietro automaticamente
        transform.DOMove(_endPosition.position, _duration)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo)
                 .OnUpdate(() =>
                 {
                     _deltaPosition = transform.position - _lastPosition;
                     _lastPosition = transform.position;
                 });
    }

}


