using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    
    [SerializeField] private float _onDuration = 5f;
    [SerializeField] private float _offDuration = 3f;
    [SerializeField] private float alertCooldown = 5f;

    private float _lastAlertTime = -999f;
    private float _timer = 0f;
    private bool _isOn = false;
    private Light _spotlight;
    private SphereCollider _detectionCollider;
    private EnemyGuardAI[] _guards;

    void Start()
    {
        _spotlight = GetComponent<Light>();
        _detectionCollider = GetComponent<SphereCollider>();
        _guards = FindObjectsOfType<EnemyGuardAI>();
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
        _spotlight.enabled = true;
        _detectionCollider.enabled = true;
    }

    void TurnOffLight()
    {
        _isOn = false;
        _spotlight.enabled = false;
        _detectionCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isOn) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && player.IsMoving()) 
            {
                if (Time.time - _lastAlertTime >= alertCooldown)
                {
                    _lastAlertTime = Time.time;
                    AlertGuards(other.transform.position);
                }
            }
        }
    }

    private void AlertGuards(Vector3 playerPosition)
    {
        foreach (var guard in _guards)
        {
           guard.ChasePosition(playerPosition);
        }
    }
}
