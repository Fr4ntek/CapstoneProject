using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FanWind : MonoBehaviour
{
    [Header("Wind Setup")]
    [SerializeField] private float _windForce = 5f; 
    [SerializeField] private Vector3 _windDirection;

    [Header("Fan Setup")]
    [SerializeField] private GameObject _fan;
    [SerializeField] private float _rotationSpeed = 360f;

    private PlayerController_CC _playerController;  
    private bool _playerInRange = false;

    private void Update()
    {

        _fan.transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        if (_playerInRange && _playerController != null)
        {
            Vector3 globalWindDir = transform.TransformDirection(_windDirection.normalized);
            _playerController.ApplyWindForce(globalWindDir * _windForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController = other.GetComponent<PlayerController_CC>();
            if (_playerController != null)
                _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _playerInRange)
        {
            _playerInRange = false;
            _playerController = null;
        }
    }

    
}
