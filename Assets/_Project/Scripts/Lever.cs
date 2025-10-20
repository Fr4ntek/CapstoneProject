using UnityEngine;
using Cinemachine;
using System.Collections;

public class Lever : MonoBehaviour
{
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private GameObject _fakeFloor;
    [SerializeField] private Animation _doorAnimation;
    [SerializeField] private bool _wrongLever;

    [Header("Camera Focus (Cinemachine)")]
    [SerializeField] private CinemachineVirtualCamera _doorCam;
    [SerializeField] private CinemachineFreeLook _playerCam;
    [SerializeField] private float _focusDuration = 2f;

    private Animation _leverAnimation;
    private bool _playerInTrigger = false;
    private bool _activated = false;

    private void Start()
    {
        _leverAnimation = GetComponent<Animation>();
    }

    private void Update()
    {
        if (!_activated && _playerInTrigger && Input.GetKeyDown(_interactKey))
        {
            Activate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInTrigger = false;
    }

    private void Activate()
    {
        _activated = true;
        _leverAnimation.Play();

        if (_wrongLever)
        {
            Destroy(_fakeFloor);
        }
        else
        {
            _doorAnimation.Play();
            StartCoroutine(FocusOnDoor());
        }
    }

    private IEnumerator FocusOnDoor()
    { 
        _doorCam.Priority = 11;
        yield return new WaitForSeconds(_focusDuration);
        _doorCam.Priority = 5;
    }
}
