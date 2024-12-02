using UnityEngine;

public class RotationAttackState : EnemyState
{
    private float _timer;

    public RotationAttackState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _timer = _enemy.RotationAttackInterval;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0) // Rotate if have fuel
        {
            _enemy.SetState(new RechargeState(_enemy));
        }
        else
        {
            if (_enemy.PlayerInDetectionRadius())
            {
                _enemy.Agent.isStopped = false;
                _enemy.RotateAttack();
            }
            else
            {
                // Enemy stops if it doesnt see the player
                _enemy.Agent.isStopped =  true;
                _enemy.Rotate();
            }
        }
    }

    public override void Exit()
    {
        _enemy.Agent.isStopped = false;
        // Optional: Cleanup idle-specific actions
    }
}