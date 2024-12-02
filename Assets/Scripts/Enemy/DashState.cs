using UnityEngine;

public class DashState : EnemyState
{
    private float _timer;
    private Vector3 _dashDirection;
    
    public DashState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _timer = _enemy.DashAttackInterval;
        
        _dashDirection = _enemy.GetPlayerDirection().normalized;
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
            _enemy.LookInDirection(_dashDirection);
            _enemy.Dash(_dashDirection);
        }
    }

    public override void Exit()
    {
    }
}