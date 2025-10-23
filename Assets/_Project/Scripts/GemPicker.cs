using DG.Tweening;
using UnityEngine;

public class GemPicker : MonoBehaviour
{
    public enum GemType { Red, Yellow, Blue }

    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private GemType _gemType;
    [SerializeField] private FinalDoorController _door;

    private UIController _uiController;

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

        _uiController = other.GetComponent<UIController>();
        _uiController.UpdateGemUI(_gemType);

        _door.CheckAllGemsCollected(_gemType);
        
        DOTween.Kill(transform);
        Destroy(gameObject);
    }
}

