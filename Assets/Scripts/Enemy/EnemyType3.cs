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
        // Perform the rotation
        Rotate();

        if (_player != null)
        {
            Vector3 playerPosition = _player.transform.position;

            // Use NavMeshAgent to handle pathfinding
            Agent.SetDestination(playerPosition);

            // Check for potential wall collisions
            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, 1f, LayerMask.GetMask("Obstacle"));

            if (hit.collider != null)
            {
                // Wall detected: Adjust direction to avoid the wall
                Vector3 avoidanceDirection = Vector3.Cross(hit.normal, Vector3.forward).normalized;
                Agent.Move(avoidanceDirection * (ChaseSpeed * Time.deltaTime));
            }
            else
            {
                // No wall detected: Continue chasing the player
                Vector3 moveDirection = directionToPlayer * (ChaseSpeed * Time.deltaTime);
                Agent.Move(moveDirection);
            }
        }
        else
        {
            // If player is not detected, fallback to idle rotation
            Rotate();
        }
    }


    public override void ChooseAttack()
    {
        SetState(new RotationAttackState(this));
    }
}