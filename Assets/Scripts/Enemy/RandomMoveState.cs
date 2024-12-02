using UnityEngine;

public class RandomMoveState : EnemyState
{
    private float _timer;
    private Vector3 _randomDirection;

    public RandomMoveState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _timer = _enemy.RandomMoveInterval;
        PickRandomDestination();
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        
        // Look in direction enemy is heading
        _enemy.LookInDirection(_randomDirection);

        if (_timer <= 0 || _enemy.ReachedDestination())
        {
            PickRandomDestination();
            _timer = _enemy.RandomMoveInterval;
        }

        if (_enemy.PlayerInDetectionRadius())
        {
            _enemy.ChooseAttack();
        }
    }

    public override void Exit()
    {
        // Optional: Cleanup random-move-specific actions
    }

    private void PickRandomDestination()
    {
        Vector3 randomDestination = _enemy.PickRandomDestination(); // Uses Enemy's NavMesh functionality
        _randomDirection = (randomDestination - _enemy.transform.position).normalized;
    }
}
