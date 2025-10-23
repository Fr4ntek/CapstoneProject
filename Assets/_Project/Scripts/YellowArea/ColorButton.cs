using System.Collections;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    [SerializeField] private string _colorName;
    [SerializeField] private ColorPuzzleManager _manager;

    private Animation _pressAnim;
    private bool _canPress = true;

    private void Start()
    {
        _pressAnim = GetComponentInParent<Animation>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_canPress && other.CompareTag("Player"))
        {
            _pressAnim?.Play();
            AudioManager.Instance.Play("ButtonPress");
            _manager.PressColor(_colorName);
            StartCoroutine(DisablePressFor(0.5f));
        }
    }

    private IEnumerator DisablePressFor(float seconds)
    {
        _canPress = false;
        yield return new WaitForSeconds(seconds);
        _canPress = true;
    }
}
