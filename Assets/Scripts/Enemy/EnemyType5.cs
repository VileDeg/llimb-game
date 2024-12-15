using UnityEngine;

public class EnemyType5 : Enemy
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private int _ammoNumber = 10;
    [SerializeField] private float _projectileSpeed = 7;
    [SerializeField] private Transform _firePoint;
    
    public int AmmoNumber => _ammoNumber;
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
        SetState(new ShootState(this));
    }
}