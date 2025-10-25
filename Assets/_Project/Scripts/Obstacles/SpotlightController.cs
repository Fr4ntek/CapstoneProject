using UnityEngine;
using static Unity.VisualScripting.Member;

public class SpotlightController : MonoBehaviour
{
    [SerializeField] private AudioClip _onSound;
    [SerializeField] private AudioClip _offSound;
    [SerializeField] private float _onDuration = 5f;
    [SerializeField] private float _offDuration = 3f;

    private float _timer = 0f;
    private bool _isOn = false;
    private Light _spotlight;
    private AudioSource _source;
    private SphereCollider _detectionCollider;
    private EnemyGuardAI[] _guards;

    void Start()
    {
        _spotlight = GetComponent<Light>();
        _detectionCollider = GetComponent<SphereCollider>();
        _guards = FindObjectsOfType<EnemyGuardAI>();
        _source = GetComponent<AudioSource>();
        TurnOffLight();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_isOn && _timer >= _onDuration)
        {
            TurnOffLight();
            _timer = 0;
        }
        else if (!_isOn && _timer >= _offDuration)
        {
            TurnOnLight();
            _timer = 0;
        }
    }

    void TurnOnLight()
    {
        _isOn = true;
        _source.PlayOneShot(_onSound);
        _spotlight.enabled = true;
        _detectionCollider.enabled = true;
    }

    void TurnOffLight()
    {
        _isOn = false;
        _source.PlayOneShot(_offSound);
        _spotlight.enabled = false;
        _detectionCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isOn) return;

        if (other.CompareTag("Player"))
        {
            PlayerController_CC player = other.GetComponent<PlayerController_CC>();
            if (player != null && player.IsMoving()) 
            {
                if (!_guards[0].IsAlerted())
                {
                    AlertGuards(other.transform.position);
                }
            }
        }
    }

    private void AlertGuards(Vector3 playerPosition)
    {
        AudioManager.Instance.Play("Alarm");
        foreach (var guard in _guards)
        {
           guard.ChasePosition(playerPosition);
        }
    }
}
