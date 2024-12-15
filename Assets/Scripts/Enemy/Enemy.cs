using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _normalMoveSpeed = 1.5f;
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _randomMoveInterval = 2f;
    [SerializeField] private float _rechargeInterval = 3f;

    public NavMeshAgent Agent { get; private set; }
    public float ChaseSpeed => _chaseSpeed;
    public float DetectionRadius => _detectionRadius;
    public float RandomMoveInterval => _randomMoveInterval;
    public float RechargeInterval => _rechargeInterval;
    public GameObject _player;
    protected EnemyState _currentState;

    protected Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    private void Start()
    {
        _player = FindAnyObjectByType<Player>().gameObject;
        SetState(new IdleState(this));
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void SetState(EnemyState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public virtual bool PlayerInDetectionRadius()
    {
        if (_player == null) return false;

        Vector3 playerPosition = _player.transform.position;
        Vector3 enemyPosition = transform.position;

        // Check if the player is within the detection radius
        float distanceToPlayer = Vector3.Distance(enemyPosition, playerPosition);
        if (distanceToPlayer > _detectionRadius) return false;

        // Perform a line-of-sight check
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, playerPosition - enemyPosition, distanceToPlayer, LayerMask.GetMask("Obstacle"));
        if (hit.collider != null)
        {
            // Obstacle detected, player not in sight
            return false;
        }

        // Player is visible and within range
        return true;
    }




    public Vector3 PickRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _detectionRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, _detectionRadius, NavMesh.AllAreas))
        {
            Agent.SetDestination(navHit.position);
            Agent.speed = _normalMoveSpeed;
        }

        return randomDirection;
    }

    public void ChasePlayer()
    {
        if (_player != null)
        {
            Vector2 playerPosition = _player.transform.position;
            Vector2 enemyPosition = transform.position;

            // Calculate direction to the player
            Vector2 direction = (playerPosition - enemyPosition).normalized;

            // Ensure the enemy stops at a safe distance from the player
            float distanceToPlayer = Vector2.Distance(playerPosition, enemyPosition);
            float safeDistance = 1f; // Minimum distance to maintain from the player

            if (distanceToPlayer > safeDistance)
            {
                Agent.SetDestination(playerPosition);
                Agent.speed = _chaseSpeed;
                LookInDirection(direction);
            }
            else
            {
                // Stop moving to prevent overlapping
                Agent.isStopped = true;
            }
        }
    }
    public bool CanSeePlayer()
    {
        if (_player == null) return false;

        // Calculate the direction to the player
        Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;

        // Perform a raycast to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, _detectionRadius, LayerMask.GetMask("Obstacle", "Player"));

        // Check if the raycast hit the player
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        // The player is not visible (an obstacle is in the way)
        return false;
    }

    public bool ReachedDestination()
    {
        return !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance;
    }

    public void LookInDirection(Vector3 direction)
    {
        direction.Normalize();
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);  // Only rotate around the Z-axis
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _chaseSpeed); // Smooth rotation
    }

    public Vector2 GetPlayerDirection()
    {
        if (_player == null) // Check if the player is null
        {
            Debug.LogWarning("Player reference is null. Cannot get direction.");
            return Vector2.zero; // Return a default direction
        }

        Vector2 direction = (_player.transform.position - transform.position).normalized;
        return direction;
    }

    public virtual void ChooseAttack() { }
}
