using UnityEngine;

public class CannonFire : MonoBehaviour
{
    [SerializeField] private BombPool _bombPool;
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _launchForce = 10f;
    [SerializeField] private float _launchAngle = 45f;
    [SerializeField] private Transform _firePoint;

    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _fireRate)
        {
            _timer = 0;
            LaunchBomb();
        }
    }

    private void LaunchBomb()
    {
        GameObject bomb = _bombPool.GetBomb();
        bomb.transform.position = _firePoint.position;

        // reset della fisica
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // passo il riferimento alla pool e parte routine di esplosione
        bomb.GetComponent<Bomb>().Initialize(_bombPool);

        // Calcola la direzione ad arco
        float rad = _launchAngle * Mathf.Deg2Rad;
        Vector3 direction = -transform.forward * Mathf.Cos(rad) + Vector3.up * Mathf.Sin(rad);
        rb.AddForce(direction.normalized * _launchForce, ForceMode.VelocityChange);
    }
}

