using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float normalMoveSpeed = 1.5f;
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private float randomMoveInterval = 2f;

    private GameObject player;
    private Vector2 randomDirection;
    private float randomMoveTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        randomMoveTimer = randomMoveInterval;
        PickRandomDirection();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            RandomMovement();
        }
    }

    private void ChasePlayer()
    {
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        RotateInDirection(directionToPlayer);
        transform.position += (Vector3)(directionToPlayer * chaseSpeed * Time.deltaTime);
    }

    private void RandomMovement()
    {
        randomMoveTimer -= Time.deltaTime;

        if (randomMoveTimer <= 0)
        {
            PickRandomDirection();
            randomMoveTimer = randomMoveInterval;
        }
        
        RotateInDirection(randomDirection);
        transform.position += (Vector3)(randomDirection * normalMoveSpeed * Time.deltaTime);
    }

    private void PickRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        randomDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
    
    private void RotateInDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
    
    // For debugging puproses
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    private void AimAndShoot()
    {
       // TODO
    }

    private void Shoot()
    {
        // TODO
    }
}
