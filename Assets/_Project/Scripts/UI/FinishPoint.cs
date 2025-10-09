using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private GameObject _showMessage;

    private bool _isPlayerInside = false;
    private bool _isDoorOpen = false;
    private UIController _UIController;
    private Animator _doorAnimator;
    private AudioSource _sfx;

    private void Start()
    {
        _doorAnimator = GetComponent<Animator>();
        _sfx = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (_isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            _UIController.ShowVictoryUI();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            _isDoorOpen = true;
            _doorAnimator.SetBool("openDoor", _isDoorOpen);
            _showMessage?.SetActive(true);
            _sfx.Play();
            _UIController = other.GetComponent<UIController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _isDoorOpen = false;
            _doorAnimator.SetBool("openDoor", _isDoorOpen);
            _showMessage?.SetActive(false);
        }
    }

}
