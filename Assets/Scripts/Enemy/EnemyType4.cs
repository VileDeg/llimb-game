using UnityEngine;

public class EnemyType4 : Enemy
{
    public override void ChooseAttack()
    {
        if (!(_currentState is ChaseState)) // Prevent switching if already in ChaseState
        {
            SetState(new ChaseState(this));
        }
    }

}