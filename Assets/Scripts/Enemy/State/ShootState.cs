using UnityEngine;

public class ShootState : EnemyState
{
    private EnemyType5 _enemyType5;
    private int _ammoCount;
    private float _shootTimer = 0.0f;
    private float _shootInterval = 0.2f;

    public ShootState(EnemyType5 enemy) : base(enemy)
    {
        _enemyType5 = enemy;
    }

    public override void Enter()
    {
        Debug.Log($"{_enemy.name} has entered ShootState.");
        _enemy.Agent.isStopped = true; // Stop moving while shooting
        _ammoCount = _enemyType5.AmmoNumber; // Initialize ammo count
    }

    public override void Update()
    {
        if (_enemyType5.PlayerInDetectionRadius() && HasLineOfSight())
        {
            // Shoot at the player if there's line of sight
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0f && _ammoCount > 0)
            {
                _shootTimer = _shootInterval; // Reset timer
                _ammoCount--; // Decrease ammo count
                _enemyType5.ShootPlayer();
            }

            // Reload if out of ammo
            if (_ammoCount <= 0)
            {
                _enemy.SetState(new RechargeState(_enemyType5));
            }
        }
        else
        {
            // Transition to ChaseState if no line of sight
            _enemy.SetState(new ChaseState(_enemy));
        }
    }

    public override void Exit()
    {
        Debug.Log($"{_enemy.name} has exited ShootState.");
        _enemy.Agent.isStopped = false; // Resume movement
    }

    private bool HasLineOfSight()
    {
        if (_enemyType5._player == null) return false; // Ensure player is not null

        Vector3 playerPosition = _enemyType5._player.transform.position;
        Vector3 enemyPosition = _enemyType5.transform.position;
        Vector3 directionToPlayer = playerPosition - enemyPosition;

        // Perform raycast to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, directionToPlayer.normalized, directionToPlayer.magnitude, LayerMask.GetMask("Obstacle"));

        return hit.collider == null; // True if no obstacle blocks the way
    }
}
