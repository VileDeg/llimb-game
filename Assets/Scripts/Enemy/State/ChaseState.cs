public class ChaseState : EnemyState
{
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (_enemy.PlayerInDetectionRadius() && _enemy is EnemyType5 enemyType5 && enemyType5.HasLineOfSight())
        {
            // Transition to ShootState if the player is visible
            _enemy.SetState(new ShootState(enemyType5));
        }
        else
        {
            // Chase the player
            _enemy.ChasePlayer();
        }
    }

    public override void Exit()
    {

    }
}
