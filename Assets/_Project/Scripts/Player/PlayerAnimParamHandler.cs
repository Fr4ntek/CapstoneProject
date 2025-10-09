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

    private Animator _anim;
    private PlayerController _playerController;
    private Rigidbody _rb;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (_playerController.IsRunning)
        {
            _anim.SetFloat(_paramNameV, _playerController.Vertical * 2);
            _anim.SetFloat(_paramNameH, _playerController.Horizontal * 2);
        }
        else
        {
            _anim.SetFloat(_paramNameV, _playerController.Vertical);
            _anim.SetFloat(_paramNameH, _playerController.Horizontal);
        }
        _anim.SetFloat(_paramNameVSpeed, _rb.velocity.y);
    }

    public void OnJump()
    {
        _anim.SetTrigger(_paramNameJump);
    }

    public void OnIsGroundedChanged(bool isGrounded)
    {
        _anim.SetBool(_paramNameIsGrounded, isGrounded);
    }

    
}
