using UnityEngine;

public class RechargeState : EnemyState
{
    private float _timer; // Timer for recharging

    public RechargeState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{_enemy.name} has entered RechargeState.");
        if (_enemy is EnemyType1 enemyType1)
        {
            _timer = enemyType1._dashRechargeInterval;
        }
        else if (_enemy is EnemyType5 enemyType5)
        {
            _timer = enemyType5._shootRechargeInterval;
        }
        else
        {
            _timer = _enemy.RechargeInterval; // Default for other enemy types
        }
        _enemy.Agent.isStopped = true;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            // Let the specific enemy type decide what to do next
            _enemy.ChooseAttack();
        }
    }

    public override void Exit()
    {
        Debug.Log($"{_enemy.name} has exited RechargeState.");
        _enemy.Agent.isStopped = false; // Resume movement
    }
}
