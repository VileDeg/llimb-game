using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashAttackInterval = 2f;
    
    public float DashAttackInterval => _dashAttackInterval;

    public void Dash(Vector3 direction)
    {        
        if (_player != null)
        {
            // Move in straight line
            Agent.Move(direction * (_dashSpeed * Time.deltaTime));
        }
    }

    public override void ChooseAttack()
    {
        SetState(new DashState(this));
    }
}
