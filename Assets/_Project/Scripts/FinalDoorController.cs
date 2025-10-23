using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class FinalDoorController : MonoBehaviour
{
    [SerializeField] private ParticleSystem exitGlow;
    [SerializeField] private CinemachineVirtualCamera _doorCam;
    [SerializeField] private float _focusDuration = 2f;

    private bool _isOpened;
    private bool _isRedGemCollected = false;
    private bool _isYellowGemCollected = false;
    private bool _isBlueGemCollected = false;

    public void CheckAllGemsCollected(GemPicker.GemType color)
    {
        if (_isOpened) return;

        switch (color)
        {
            case GemPicker.GemType.Red:
                _isRedGemCollected = true;
                break;
            case GemPicker.GemType.Yellow:
                _isYellowGemCollected = true;
                break;
            case GemPicker.GemType.Blue:
                _isBlueGemCollected = true;
                break;
        }

        if(_isRedGemCollected && _isBlueGemCollected && _isYellowGemCollected) 
            OpenDoor();
        
    }

    private void OpenDoor()
    {
        _isOpened = true;
        GetComponentInParent<Animation>().Play();
        GetComponent<Collider>().enabled = false;
        exitGlow.Play();
        StartCoroutine(FocusOnDoor());
    }

    private IEnumerator FocusOnDoor()
    {
        _doorCam.Priority = 11;
        yield return new WaitForSeconds(_focusDuration);
        _doorCam.Priority = 5;
    }
}
