using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private int _lifeTime = 3;
    [SerializeField] private int _damage = 1;

    private Rigidbody _rb;
    private ShootController _shootController;

    public void Setup(ShootController shootController)
    {
        _shootController = shootController;
    }

    public void ReturnToPool()
    {
        if (!gameObject.activeInHierarchy) return; //non serve, ma era per capire se invoke veniva chiamata anche su oggetti disabilitati

        _shootController.ReleaseBullet(this);
    }

    public void Shoot(Vector3 direction)
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = direction * _speed;
        CancelInvoke(); // per sicurezza, perchè potrebbe chiamare l'invoke del bullet precedente
        Invoke("ReturnToPool", _lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            collision.collider.gameObject.GetComponent<LifeController>().TakeDamage(_damage);
        }
        ReturnToPool();
    }
}
