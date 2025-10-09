using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private TurretBullet _bulletPrefab;
    [SerializeField] private Transform _parent;
    
    private Queue<TurretBullet> _bulletPool = new Queue<TurretBullet>();
    private float _lastShotTime = 0.0f;
    private bool _isRanged = false;

    void Update()
    {
        if (CanShoot() && _isRanged)
        {
            TurretBullet b = GetBullet();
            b.transform.position = _spawnPoint.position;
            b.Shoot(_spawnPoint.up);
        }
    }
    private bool CanShoot()
    {
        if (Time.time - _lastShotTime > _fireRate)
        {
            _lastShotTime = Time.time;
            return true;
        }
        return false;
    }

    public TurretBullet GetBullet()
    {
        TurretBullet bullet = null;
        if (_bulletPool.Count > 0)
        {
            bullet = _bulletPool.Dequeue();
            bullet.gameObject.SetActive(true);
        }
        else
        {
            bullet = Instantiate(_bulletPrefab,_parent);
            bullet.Setup(this);
        }
        return bullet;
    }

    public void ReleaseBullet(TurretBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) _isRanged = true;
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _isRanged = false;
    }
}
