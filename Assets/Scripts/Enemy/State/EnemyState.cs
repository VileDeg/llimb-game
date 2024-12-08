using UnityEngine;

public abstract class EnemyState
{
    protected Enemy _enemy;

    public EnemyState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
