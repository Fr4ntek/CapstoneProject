using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Renderer _laserRenderer;
    public Collider _laserCollider;
    public float _onTime = 1.5f;
    public float _offTime = 1f;

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
            // esempio: player muore o viene respawnato
        }
    }
}
