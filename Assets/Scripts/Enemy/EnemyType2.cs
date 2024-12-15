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
        if (_player == null) // Check if the player is null
        {
            Debug.LogWarning("Player reference is null. Cannot shoot.");
            return; // Exit early if there's no player to shoot at
        }
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
        if (!(_currentState is ChaseShootState)) // Only set state if not already in ChaseShootState
        {
            SetState(new ChaseShootState(this));
        }
    }

}