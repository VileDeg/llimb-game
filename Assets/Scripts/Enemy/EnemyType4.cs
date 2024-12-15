using UnityEngine;

public class EnemyType4 : Enemy
{
    public override void ChooseAttack()
    {
        SetState(new ChaseState(this));
    }
}