using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private GameObject _showMessage;

    private bool _isPlayerInside = false;
    private UIController _uiController;
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
            //_UIController.ShowVictoryUI();
            //vai al secondo livello
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            _doorAnimator.SetBool("openDoor", true);
            _showMessage?.SetActive(true);
            _sfx.Play();
            _uiController = other.GetComponent<UIController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _doorAnimator.SetBool("openDoor", false);
            _showMessage?.SetActive(false);
        }
    }

}
