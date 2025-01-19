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

    
}
