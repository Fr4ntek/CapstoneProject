using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _onTime = 1.5f;
    [SerializeField] private float _offTime = 1f;

    private Renderer _laserRenderer;
    private Collider _laserCollider;
    
    void Start() 
    {
        _laserRenderer = GetComponent<Renderer>();  
        _laserCollider = GetComponent<Collider>();
        StartCoroutine(ToggleLaser());
    } 

    IEnumerator ToggleLaser()
    {
        while (true)
        {
            _laserRenderer.enabled = true;
            _laserCollider.enabled = true;
            yield return new WaitForSeconds(_onTime);
            _laserRenderer.enabled = false;
            _laserCollider.enabled = false;
            yield return new WaitForSeconds(_offTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
    }
}
