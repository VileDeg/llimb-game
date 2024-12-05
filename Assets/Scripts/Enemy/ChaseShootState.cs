using UnityEngine;

public class ChaseShootState : EnemyState
{
    private float _timer; 

    private float _shootTimer = 0.0f;
    private float _shootInterval = 0.5f;
    public ChaseShootState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _timer = _enemy.ShootingInterval;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        _shootTimer -= Time.deltaTime;


        if (_timer <= 0)
        {
            _enemy.SetState(new RechargeState(_enemy));
        }
        else
        {
            _enemy.ChasePlayer();
            if (_shootTimer <= 0)
            {
                _shootTimer = _shootInterval;
                _enemy.ShootPlayer();
            }
        }
    }

    public override void Exit()
    {
        // Optional: Cleanup chase-specific actions
    }
}