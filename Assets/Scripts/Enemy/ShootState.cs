using UnityEngine;

public class ShootState : EnemyState
{
    private EnemyType5 _enemyType5;
    
    private int _ammoCount; 

    private float _shootTimer = 0.0f;
    private float _shootInterval = 0.2f;

    public ShootState(EnemyType5 enemy) : base(enemy)
    {
        _enemyType5 = enemy;
    }

    public override void Enter()
    {
        _enemy.Agent.isStopped = true;
        _ammoCount = _enemyType5.AmmoNumber;
    }

    public override void Update()
    {
        _shootTimer -= Time.deltaTime;
        
        if (_ammoCount <= 0)
        {
            _enemyType5.SetState(new RechargeState(_enemyType5));
        }
        else
        {
            _enemyType5.LookInDirection(_enemyType5.GetPlayerDirection());
            if (_shootTimer <= 0)
            {
                _ammoCount -= 1;
                _shootTimer = _shootInterval;
                _enemyType5.ShootPlayer();
            }
        }
    }

    public override void Exit()
    {
        _enemy.Agent.isStopped = false;
        // Optional: Cleanup chase-specific actions
    }
}