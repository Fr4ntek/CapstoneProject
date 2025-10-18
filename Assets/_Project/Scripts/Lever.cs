using TMPro;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private GameObject _objectToAffect;
    [SerializeField] private bool _destroyObject;
     [SerializeField] private string _triggerName = "Press";
    
    private Animator _anim; 
    private bool _activated = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_activated || !other.CompareTag("Player")) return;

        if (Input.GetKeyDown(_interactKey))
        {
            Activate();
        }
    }

    private void Activate()
    {
        _anim.SetTrigger(_triggerName);

        if (_destroyObject)
        {
            if (_objectToAffect != null) Destroy(_objectToAffect);
        }
        else
        {
            // Apre porta: attiva un’animazione o sposta fisicamente l’oggetto
            //if (_objectToAffect != null)
            //    _objectToAffect.transform.position += Vector3.up * 3f; // esempio: si alza di 3 unità
        }
        _activated = true;
    }
}
