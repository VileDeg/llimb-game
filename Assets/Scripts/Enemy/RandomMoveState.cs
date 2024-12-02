using UnityEngine;

public class RandomMoveState : EnemyState
{
    private float _timer;

    public RandomMoveState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _timer = _enemy.RandomMoveInterval;
        PickRandomDestination();
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0 || _enemy.ReachedDestination())
        {
            PickRandomDestination();
            _timer = _enemy.RandomMoveInterval;
        }

        if (_enemy.PlayerInDetectionRadius())
        {
            _enemy.SetState(new ChaseState(_enemy));
        }
    }

    public override void Exit()
    {
        // Optional: Cleanup random-move-specific actions
    }

    private void PickRandomDestination()
    {
        _enemy.PickRandomDestination(); // Uses Enemy's NavMesh functionality
    }
}
