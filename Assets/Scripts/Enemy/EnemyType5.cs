using UnityEngine;

public class EnemyType5 : Enemy
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private int _ammoNumber = 10;
    [SerializeField] private float _projectileSpeed = 7f;
    [SerializeField] private float _shootingRange = 10f;
    [SerializeField] public float _shootRechargeInterval = 2f;
    public int AmmoNumber => _ammoNumber;
    public GameObject ProjectilePrefab => _projectilePrefab;
    public Transform FirePoint => _firePoint;

    public override void ChooseAttack()
    {
        if (PlayerInDetectionRadius() && HasLineOfSight())
        {
            // Transition to ShootState if the player is within shooting range and visible
            SetState(new ShootState(this));
        }
        else
        {
            // Otherwise, chase the player
            SetState(new ChaseState(this));
        }
    }

    public void ShootPlayer()
    {
        if (_player == null)
        {
            Debug.LogWarning($"{name}: Player reference is null. Cannot shoot.");
            return; // Exit if the player is not set
        }

        // Rotate to face the player
        LookInDirection(GetPlayerDirection());

        // Spawn a projectile
        var bulletGO = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        var projectile = bulletGO.GetComponent<AProjectile>();

        if (projectile != null)
        {
            Vector3 dir = (_player.transform.position - _firePoint.position).normalized;
            projectile.SetVelocity(dir * _projectileSpeed);
        }
    }


    private new void LookInDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
