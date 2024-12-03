using UnityEngine;

public class ShootState : EnemyState
{
    private float _timer; // TODO maybe change to set ammo?
    public ShootState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _timer = _enemy.ShootingInterval;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0)
        {
            _enemy.SetState(new RechargeState(_enemy));
        }
        else
        {
            //TODO just stand and shoot or run after player and shoot?
            _enemy.ShootPlayer();
            //_enemy.ChasePlayer();
        }
    }

    public override void Exit()
    {
        // Optional: Cleanup chase-specific actions
    }
}