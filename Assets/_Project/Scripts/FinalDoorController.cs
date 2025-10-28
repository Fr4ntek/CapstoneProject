using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class FinalDoorController : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private ParticleSystem exitGlow;
    [SerializeField] private CinemachineVirtualCamera _doorCam;
    [SerializeField] private float _focusDuration = 2f;

    private bool _isOpened;
    

    private void OnEnable()
    {
        _playerStats.OnAllGemsCollected += OpenDoor;
    }

    private void OnDisable()
    {
        _playerStats.OnAllGemsCollected -= OpenDoor;
    }

    private void OpenDoor()
    {
        if (_isOpened) return;
        _isOpened = true;
        GetComponentInParent<Animation>().Play();
        GetComponent<Collider>().enabled = false;
        exitGlow.Play();
        StartCoroutine(FocusOnDoor());
    }

    private IEnumerator FocusOnDoor()
    {
        _doorCam.Priority = 12;
        yield return new WaitForSeconds(_focusDuration);
        _doorCam.Priority = 5;
    }
}
