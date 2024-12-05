using UnityEngine;

public class ChaseShootState : EnemyState
{
    private EnemyType2 _enemyType2;
    
    private float _timer; 

    private float _shootTimer = 0.0f;
    private float _shootInterval = 0.5f;

    public ChaseShootState(EnemyType2 enemy) : base(enemy)
    {
        _enemyType2 = enemy;
    }

    public override void Enter()
    {
        _timer = _enemyType2.ShootingInterval;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        _shootTimer -= Time.deltaTime;


        if (_timer <= 0)
        {
            _enemyType2.SetState(new RechargeState(_enemyType2));
        }
        else
        {
            _enemyType2.ChasePlayer();
            if (_shootTimer <= 0)
            {
                _shootTimer = _shootInterval;
                _enemyType2.ShootPlayer();
            }
        }
    }

    public override void Exit()
    {
        // Optional: Cleanup chase-specific actions
    }
}