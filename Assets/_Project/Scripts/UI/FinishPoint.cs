using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private GameObject _showMessage;
    [SerializeField] private GameManager _gameManager;

    private bool _isPlayerInside = false;
    private Animation _anim;

    private void Start()
    {
        _anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if (_isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
           _gameManager.ChangeScene("Level2");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Play("Level1Completed");
            _isPlayerInside = true;
            _anim.Play("openFirstLevelDoor");
            _showMessage?.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _anim.Play("closeFirstLevelDoor");
            _showMessage?.SetActive(false);
        }
    }
}
