using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // Optional: Set chase-specific animations or behaviors
    }

    public override void Update()
    {
        if (!_enemy.PlayerInDetectionRadius())
        {
            _enemy.SetState(new RandomMoveState(_enemy));
        }
        else
        {
            _enemy.ChasePlayer(); // Uses Enemy's NavMeshAgent to follow the player
        }
    }

    public override void Exit()
    {
        // Optional: Cleanup chase-specific actions
    }
}
