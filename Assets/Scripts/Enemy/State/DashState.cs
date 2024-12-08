using UnityEngine;

public class DashState : EnemyState
{
    private EnemyType1 _enemyType1;
    
    private float _timer;
    
    private Vector3 _dashDirection;

    public DashState(EnemyType1 enemy) : base(enemy)
    {
        _enemyType1 = enemy;
    }

    public override void Enter()
    {
        _timer = _enemyType1.DashAttackInterval;
        
        _dashDirection = _enemyType1.GetPlayerDirection().normalized;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0) // Rotate if have fuel
        {
            _enemyType1.SetState(new RechargeState(_enemyType1));
        }
        else
        {
            _enemyType1.LookInDirection(_dashDirection);
            _enemyType1.Dash(_dashDirection);
        }
    }

    public override void Exit()
    {
    }
}