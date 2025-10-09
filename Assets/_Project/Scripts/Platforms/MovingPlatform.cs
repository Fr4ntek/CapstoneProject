using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    private Vector3 _targetPos;
    private Vector3 _lastPosition;

    private void Start()
    {
        _targetPos = _pointB.position;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPos) < 0.05f)
        {
            // inverto destinazione
            _targetPos = (_targetPos == _pointA.position) ? _pointB.position : _pointA.position;
        }
    }

    public Vector3 GetVelocity()
    {
        Vector3 velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
        return velocity;
    }
}
