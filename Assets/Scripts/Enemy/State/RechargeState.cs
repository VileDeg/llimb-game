using UnityEngine;

public class RechargeState : EnemyState
{
    private float _timer;
    
    public RechargeState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _enemy.Agent.isStopped = true;
        
        _timer = _enemy.RechargeInterval;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0)
        {
            _enemy.SetState(new IdleState(_enemy));
        }
    }

    public override void Exit()
    {
        _enemy.Agent.isStopped = false;
    }
}