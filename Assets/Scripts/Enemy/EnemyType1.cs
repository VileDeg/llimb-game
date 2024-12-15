using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField] private float _dashAttackInterval = 1.5f; // Time per charge
    [SerializeField] public float _dashRechargeInterval = 1.5f; // Recharge time before the next charge
    [SerializeField] private float _dashSpeed = 10f; // Speed of the charge

    public float DashAttackInterval => _dashAttackInterval;
    public float DashRechargeInterval => _dashRechargeInterval;

    public void Dash(Vector3 direction)
    {
        if (Player != null)
        {
            // Move in a straight line
            Agent.Move(direction * (_dashSpeed * Time.deltaTime));
        }
    }

    public override void ChooseAttack()
    {
        if (PlayerInDetectionRadius() && HasLineOfSight())
        {
            // Transition to DashState if the player is detected and visible
            SetState(new DashState(this));
        }
        else
        {
            // Transition to ChaseState otherwise
            SetState(new ChaseState(this));
        }
    }

    public bool HasLineOfSight()
    {
        if (Player == null) return false;

        Vector3 playerPosition = Player.transform.position;
        Vector3 enemyPosition = transform.position;
        Vector3 directionToPlayer = playerPosition - enemyPosition;

        // Perform raycast to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, directionToPlayer.normalized, directionToPlayer.magnitude, LayerMask.GetMask("Obstacle"));

        return hit.collider == null; // True if no obstacle blocks the way
    }
}
