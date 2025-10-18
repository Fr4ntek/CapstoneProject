using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private int _jumpHeight = 3;
    [SerializeField] private float _fallMultiplier = 1;
    [SerializeField] private float _lowJumpMultiplier = 1;
    [SerializeField] private float _jumpMultiplier = 2;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private UnityEvent _onJump;

    public float Horizontal {get; private set;}
    public float Vertical {get; private set;}
    public bool IsRunning { get; private set;}

    private float _movingTimer;
    private Rigidbody _rb;
    private Vector3 _direction;
    private Vector3 _camForward;
    private Vector3 _camRight;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_groundChecker == null) _groundChecker = GetComponentInChildren<GroundChecker>();
    }

    private void Update()
    {
        if (_cameraTransform == null) return;

        // Movement
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        _camForward = _cameraTransform.forward.normalized;
        _camRight = _cameraTransform.right.normalized;
        _camForward.y = 0f;
        _camRight.y = 0f;

        _direction = (Vertical * _camForward + Horizontal * _camRight).normalized * _speed;

        if (_direction.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            float jumpForce = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
            _rb.AddForce(Vector3.up * jumpForce * _jumpMultiplier, ForceMode.VelocityChange);
            _onJump?.Invoke();
        }

        if (_rb.velocity.y < 0) 
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump")) // se lascia Spazio scende prima
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && Grounded()) 
        {
            _speed = _runSpeed;
            IsRunning = true;
        }
        else
        {
            _speed = _walkSpeed;
            IsRunning = false;
        }

    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction + new Vector3(0f, _rb.velocity.y, 0f);
    }

    private bool Grounded()
    {
        if (_groundChecker.IsGrounded) return true;
        return false;
    }

    public bool IsMoving()
    {
        if (_rb.velocity.magnitude > 1.5f)
            _movingTimer += Time.deltaTime;
        else
            _movingTimer = 0f;

        return _movingTimer > 0.2f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            MovingPlatform platform = collision.gameObject.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                Vector3 platformVelocity = platform.GetVelocity();
                _rb.MovePosition(_rb.position + platformVelocity * Time.fixedDeltaTime);
            }
        }
    }
}
