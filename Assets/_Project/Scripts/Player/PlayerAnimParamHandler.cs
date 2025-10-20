using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimParamHandler : MonoBehaviour
{
    [SerializeField] private string _paramNameV = "vertical";
    [SerializeField] private string _paramNameH = "horizontal";
    [SerializeField] private string _paramNameRun = "isRunning";
    [SerializeField] private string _paramNameVSpeed = "vSpeed";
    [SerializeField] private string _paramNameIsGrounded = "isGrounded";
    [SerializeField] private string _paramNameJump = "jump";

    private bool _groundedLastFrame = true;
    private float _groundedTimer = 0f;
    [SerializeField] private float _groundedBuffer = 0.1f; // 100ms

    private Animator _anim;
    private PlayerController_CC _player;
    // private Rigidbody _rb;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<PlayerController_CC>();
    }

    void Update()
    {
        if (_player.IsRunning)
        {
            _anim.SetFloat(_paramNameV, _player.Vertical * 2);
            _anim.SetFloat(_paramNameH, _player.Horizontal * 2);
        }
        else
        {
            _anim.SetFloat(_paramNameV, _player.Vertical);
            _anim.SetFloat(_paramNameH, _player.Horizontal);
        }
  
        _anim.SetFloat(_paramNameVSpeed, _player.VelocityY);
        _anim.SetBool(_paramNameIsGrounded, IsGroundedSmooth());

    }

    public void OnJump()
    {
        _anim.SetTrigger(_paramNameJump);
    }
    public bool IsGroundedSmooth()
    {
        if (_player.IsGrounded())
        {
            _groundedTimer = _groundedBuffer;
            _groundedLastFrame = true;
        }
        else
        {
            _groundedTimer -= Time.deltaTime;
            if (_groundedTimer <= 0f) _groundedLastFrame = false;
        }
        return _groundedLastFrame;
    }
}
