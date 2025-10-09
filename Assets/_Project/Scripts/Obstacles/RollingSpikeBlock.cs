using UnityEngine;

public class RollingSpikeBlock : MonoBehaviour
{
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _damageCooldown = 1f;
    [SerializeField] private float _triggerPoint = 1f;

    private Vector3 _respawnPosition;
    private Quaternion _respawnRotation;
    private float _lastHitTime;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _respawnPosition = transform.position;
        _respawnRotation = transform.rotation;
    }

    private void Update()
    {
        if (transform.position.y < _triggerPoint)
        {
            // Reset posizione e velocità
            transform.position = _respawnPosition;
            transform.rotation = _respawnRotation;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - _lastHitTime >= _damageCooldown)
            {
                collision.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
                _lastHitTime = Time.time;
            }
        }
    }
}
