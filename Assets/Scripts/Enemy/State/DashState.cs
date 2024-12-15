using UnityEngine;

public class DashState : EnemyState
{
    private EnemyType1 _enemyType1;
    private float _timer; // Timer for how long the enemy will charge
    private Vector3 _dashDirection; // Direction of the charge

    public DashState(EnemyType1 enemy) : base(enemy)
    {
        _enemyType1 = enemy;
    }

    public override void Enter()
    {
        Debug.Log($"{_enemy.name} has entered DashState.");

        // Check line of sight before initiating dash
        if (HasLineOfSight())
        {
            _timer = _enemyType1.DashAttackInterval; // Time before the dash ends
            _dashDirection = _enemyType1.GetPlayerDirection().normalized; // Direction toward the player
            LookInDashDirection(); // Face the correct direction
        }
        else
        {
            // Transition back to ChaseState if no line of sight
            _enemy.SetState(new ChaseState(_enemy));
        }
    }

    public override void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;

            // Continue dashing
            _enemyType1.Dash(_dashDirection);
        }
        else
        {
            // Transition to RechargeState after the dash
            _enemy.SetState(new RechargeState(_enemyType1));
        }
    }

    public override void Exit()
    {
        Debug.Log($"{_enemy.name} has exited DashState.");
    }

    private bool HasLineOfSight()
    {
        if (_enemyType1._player == null) return false; // Ensure _player is not null

        Vector3 playerPosition = _enemyType1._player.transform.position;
        Vector3 enemyPosition = _enemyType1.transform.position;
        Vector3 directionToPlayer = playerPosition - enemyPosition;

        // Perform raycast to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, directionToPlayer.normalized, directionToPlayer.magnitude, LayerMask.GetMask("Obstacle"));

        return hit.collider == null; // True if no obstacle blocks the way
    }

    private void LookInDashDirection()
    {
        if (_dashDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(_dashDirection.y, _dashDirection.x) * Mathf.Rad2Deg - 90f;
            _enemyType1.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
