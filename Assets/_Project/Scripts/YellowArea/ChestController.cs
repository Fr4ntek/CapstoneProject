using DG.Tweening;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private GameObject _gem;
    [SerializeField] private Transform _floatPoint;
    [SerializeField] private float _moveDuration = 1f;  

    private Animation _openAnim;
    private bool _opened = false;


    private void Start()
    {
        _openAnim = GetComponent<Animation>();
    }
    public void Unlock()
    {
        if (_opened) return;
        _opened = true;
        _openAnim?.Play();
        AudioManager.Instance.Play("ChestOpening");
        SpawnAndAnimateGem();
    }

    private void SpawnAndAnimateGem()
    {
        _gem.SetActive(true);
        _gem.transform.DOMove(_floatPoint.position, _moveDuration)
            .SetEase(Ease.OutCubic);

    }
}
