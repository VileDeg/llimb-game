using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // Optional: Set idle-specific animations or behaviors
    }

    public override void Update()
    {
        if (_enemy.PlayerInDetectionRadius())
        {
            if (_enemy.Type == EnemyType.Type3)
            {
                _enemy.SetState(new RotationAttackState(_enemy));
            }
            else
            {
                _enemy.SetState(new ChaseState(_enemy));
            }
        }
        else
        {
            Debug.Log("Random movement from idel");
            _enemy.SetState(new RandomMoveState(_enemy));
        }
    }

    public override void Exit()
    {
        // Optional: Cleanup idle-specific actions
    }
}
