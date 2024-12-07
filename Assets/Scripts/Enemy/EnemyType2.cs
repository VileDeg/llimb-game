using UnityEngine;

public class EnemyType2 : Enemy
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _shootingInterval = 4f;
    [SerializeField] private float _projectileSpeed = 25;
    [SerializeField] private Transform _firePoint;
    
    public float ShootingInterval => _shootingInterval;
    public GameObject ProjectilePrefab => _projectilePrefab;
    public Transform FirePoint => _firePoint;
    
    public void ShootPlayer()
    {
        LookInDirection(GetPlayerDirection());

        var bulletGO = Instantiate(
            _projectilePrefab,
            _firePoint.position,
            Quaternion.identity);

        var projectile = bulletGO.GetComponent<AProjectile>();

        var dir = (_firePoint.position - transform.position).normalized;

        projectile.SetVelocity(dir * _projectileSpeed);
    }


    public override void ChooseAttack()
    {
        SetState(new ChaseShootState(this));
    }
}