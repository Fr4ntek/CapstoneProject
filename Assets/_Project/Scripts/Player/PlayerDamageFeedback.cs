using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerDamageFeedback : MonoBehaviour
{
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private float _flashDuration = 0.1f;
    [SerializeField] private List<AudioClip> _painSounds = new List<AudioClip>();

    private Color[] _originalColors;
    private AudioSource _source;
    private Material[] _mats;
    private Tween[] _currentTweens;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _mats = GetComponent<Renderer>().materials;

        _originalColors = new Color[_mats.Length];
        for (int i = 0; i < _mats.Length; i++)
            _originalColors[i] = _mats[i].color;
    }

    public void FlashDamage()
    {
        if (_currentTweens != null)
        {
            foreach (var t in _currentTweens)
                t.Kill();
        }

        _currentTweens = new Tween[_mats.Length];

        for (int i = 0; i < _mats.Length; i++)
        {
            int index = i;
            _currentTweens[index] = _mats[index].DOColor(_damageColor, _flashDuration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .OnKill(() => _mats[index].color = _originalColors[index]);
        }

        _source.PlayOneShot(_painSounds[Random.Range(0, _painSounds.Count)]);
    }
}
