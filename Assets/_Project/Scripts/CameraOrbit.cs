using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform _target;       
    [SerializeField] private float _distance = 10f;       
    [SerializeField] private float _xSpeed = 180f;      
    [SerializeField] private float _ySpeed = 120f;       
    [SerializeField] private float _yMin = -20f;         
    [SerializeField] private float _yMax = 80f;

    private float _yaw; // mouse X
    private float _pitch; // mouse Y 

    void Start()
    {
        if (_target == null) return;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (_target == null) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * _xSpeed * Time.deltaTime;
        _pitch -= mouseY * _ySpeed * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, _yMin, _yMax);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -_distance);

        transform.position = _target.position + offset;
        transform.LookAt(_target.position);
    }
}
