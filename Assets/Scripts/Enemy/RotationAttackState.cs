using UnityEngine;

public class RotationAttackState : EnemyState
{
    private EnemyType3 _enemyType3;
    private float _timer;

    public RotationAttackState(EnemyType3 enemy) : base(enemy)
    {
        _enemyType3 = enemy;
    }

    public override void Enter()
    {
        _timer = _enemyType3.RotationAttackInterval;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0) // Rotate if have fuel
        {
            _enemyType3.SetState(new RechargeState(_enemyType3));
        }
        else
        {
            if (_enemyType3.PlayerInDetectionRadius())
            {
                _enemyType3.Agent.isStopped = false;
                _enemyType3.RotateAttack();
            }
            else
            {
                // Enemy stops if it doesnt see the player
                _enemyType3.Agent.isStopped =  true;
                _enemyType3.Rotate();
            }
        }
    }

    public override void Exit()
    {
        _enemyType3.Agent.isStopped = false;
    }
}