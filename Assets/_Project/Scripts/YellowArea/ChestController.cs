using DG.Tweening;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [Header("Gem Settings")]
    [SerializeField] private GameObject _gem;
    //[SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _floatPoint;
    [SerializeField] private float _moveDuration = 1f;  

    private Animation _openAnim;
    private bool _opened = false;
   // private GameObject _gemInstance;

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
        //if (_gemPrefab == null || _spawnPoint == null || _floatPoint == null) return;

        //_gemInstance = Instantiate(_gemPrefab, _spawnPoint.position, _spawnPoint.rotation);
        //_gemInstance.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        _gem.SetActive(true);
        _gem.transform.DOMove(_floatPoint.position, _moveDuration)
            .SetEase(Ease.OutCubic);

    }
}
