using UnityEngine;

public class ChaseShootState : EnemyState
{
    private EnemyType2 _enemyType2;

    private float _shootTimer = 0.0f; // Timer for shooting intervals
    private float _shootInterval = 0.5f; // Time between shots

    public ChaseShootState(EnemyType2 enemy) : base(enemy)
    {
        _enemyType2 = enemy;
    }

    public override void Enter()
    {
        Debug.Log($"{_enemy.name} has entered ChaseShootState.");
    }

    public override void Update()
    {
        // Always chase the player
        _enemyType2.ChasePlayer();

        // Update shooting timer
        _shootTimer -= Time.deltaTime;

        // Check if it's time to shoot
        if (_shootTimer <= 0f)
        {
            _shootTimer = _shootInterval; // Reset timer
            _enemyType2.ShootPlayer();    // Shoot at the player
        }
    }

    public override void Exit()
    {
        Debug.Log($"{_enemy.name} has exited ChaseShootState."); // This should rarely happen in this locked behavior
    }
}
