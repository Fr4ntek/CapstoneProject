using UnityEngine;

public class CannonMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _limitX = 5f;

    private int _direction = 1;
    private float _currentPos;
    private void Start()
    {
        _currentPos = transform.position.x;
    }
    void Update()
    {
        transform.Translate(Vector3.right * _speed * _direction * Time.deltaTime);
        if (transform.position.x > _currentPos + _limitX) 
            _direction = -1;
        else if (transform.position.x < _currentPos - _limitX) 
            _direction = 1;
    }
}
