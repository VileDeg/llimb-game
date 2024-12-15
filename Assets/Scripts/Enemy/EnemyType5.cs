using UnityEngine;

public class EnemyType5 : Enemy
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private int _ammoNumber = 10;
    [SerializeField] private float _projectileSpeed = 7f;
    [SerializeField] private float _shootingRange = 10f; // Extended range for entering ShootState
    [SerializeField] public float _shootRechargeInterval = 2f;
    public int AmmoNumber => _ammoNumber;
    public GameObject ProjectilePrefab => _projectilePrefab;
    public Transform FirePoint => _firePoint;

    public override bool PlayerInDetectionRadius()
    {
        if (_player == null) return false;

        // Use the extended shooting range instead of the default detection radius
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        return distanceToPlayer <= _shootingRange;
    }

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

    public bool HasLineOfSight()
    {
        if (_player == null) return false; // Ensure _player is not null

        Vector3 playerPosition = _player.transform.position;
        Vector3 enemyPosition = transform.position;
        Vector3 directionToPlayer = playerPosition - enemyPosition;

        // Perform raycast to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, directionToPlayer.normalized, directionToPlayer.magnitude, LayerMask.GetMask("Obstacle"));

        return hit.collider == null; // True if no obstacle blocks the way
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
