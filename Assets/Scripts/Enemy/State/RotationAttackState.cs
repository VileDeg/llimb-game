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
        Debug.Log($"{_enemy.name} has entered RotationAttackState.");
        _timer = _enemyType3.RotationAttackInterval; // Duration of the attack
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;

        // Continuously rotate and attack regardless of timer
        _enemyType3.RotateAttack();
    }

    public override void Exit()
    {
        Debug.Log($"{_enemy.name} has exited RotationAttackState.");
    }
}
