using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    [SerializeField] private int _damage = 20;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    private Vector3 _targetPos;

    private void Start()
    {
        _targetPos = _pointB.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) < 0.05f)
        {
            // inverto destinazione
            _targetPos = (_targetPos == _pointA.position) ? _pointB.position : _pointA.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
    }
}
