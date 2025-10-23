using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_CC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Jump/Gravity")]
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _fallMultiplier = 2f;

    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private UnityEvent _onJump;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool IsRunning { get; private set; }
    public float VelocityY { get; private set; }

    private CharacterController _cc;
    private MovingPlatform _currentPlatform;
    private Vector3 _externalForces = Vector3.zero;
    private Vector3 _velocity;  
    private Vector3 _direction;
    private Vector3 _move;
    private Vector3 _camForward, _camRight;
    private float _movingTimer;

    private void Start()
    {
        _cc = GetComponent<CharacterController>();
        if (_cameraTransform == null)
            _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Input
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        _camForward = _cameraTransform.forward;
        _camRight = _cameraTransform.right;
        _camForward.y = 0f;
        _camRight.y = 0f;
        _camForward.Normalize();
        _camRight.Normalize();

        _direction = (Vertical * _camForward + Horizontal * _camRight).normalized;

        // Speed & Running
        float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
        IsRunning = speed > _walkSpeed;
        _move = _direction * speed;

        // Rotation 
        if (_direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.deltaTime);

        // Grounded & Jump
        if (IsGrounded())
        {
            if (_velocity.y < 0f) _velocity.y = -0.1f;

            if (Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                _onJump?.Invoke();
            }
        }
        else
        {
            // Fall multiplier per velocizzare caduta
            if (_velocity.y < 0)
                _velocity.y += _gravity * _fallMultiplier * Time.deltaTime;
            else
                _velocity.y += _gravity * Time.deltaTime;
        }

        // Movement timer for spotlights
        if (_direction.magnitude > 0.1f) _movingTimer += Time.deltaTime;
        else _movingTimer = 0f;

        // Player Movement
        Vector3 horizontalMove = _move * Time.deltaTime;
        Vector3 verticalMove = _velocity * Time.deltaTime;
        Vector3 totalMove = horizontalMove + verticalMove + (_externalForces * Time.deltaTime);

        // Player on platform
        if (_currentPlatform != null)
            totalMove += _currentPlatform.DeltaPosition;

        _cc.Move(totalMove);

        _externalForces = Vector3.zero;

        if (!IsGrounded()) 
            _currentPlatform = null;
    }

    public bool IsGrounded()
    {
        return _cc.isGrounded;
    }

    public bool IsMoving()
    {
        return _movingTimer > 0.2f;
    }

    // Moving Platforms
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            _currentPlatform = hit.collider.GetComponent<MovingPlatform>();
        }
    }

    // Wind Force
    public void ApplyWindForce(Vector3 force)
    {
        _externalForces += force;
    }

}
