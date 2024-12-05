using UnityEngine;

public class EnemyType3 : Enemy
{
    [SerializeField] private float _rotationSpeed = 500f;
    [SerializeField] private float _rotationAttackInterval = 5f;
    
    public float RotationAttackInterval => _rotationAttackInterval;

    public void Rotate()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
    
    public void RotateAttack()
    {
        Rotate();

        if (_player != null)
        {
            Agent.Move(GetPlayerDirection() * (ChaseSpeed * Time.deltaTime));
        }
    }

    public override void ChooseAttack()
    {
        SetState(new RotationAttackState(this));
    }
}