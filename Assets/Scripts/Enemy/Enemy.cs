using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _normalMoveSpeed = 1.5f;
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _randomMoveInterval = 2f;
    [SerializeField] private float _rechargeInterval = 3f;

    [Header("Detection Settings")]
    [SerializeField] protected float _fovAngle = 90f; // Field of View angle in degrees
    [SerializeField] protected float _fovRange = 10f; // Detection range within the field of view
    [SerializeField] protected float _rearDetectionRange = 3f; // Detection range outside the field of view
    [SerializeField]
    private GameObject _detectionCirclePrefab; // Prefab for the circular range

    [SerializeField]
    private GameObject _detectionConePrefab; // Prefab for the 90-degree cone range

    private GameObject _circleInstance;
    private GameObject _coneInstance;
    public NavMeshAgent Agent { get; private set; }
    public float ChaseSpeed => _chaseSpeed;
    public float RandomMoveInterval => _randomMoveInterval;
    public float RechargeInterval => _rechargeInterval;
    public GameObject Player => _player;

    protected GameObject _player;
    protected EnemyState _currentState;

    private Rigidbody2D _rb;
    private EnemyDestructable _destructable;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _destructable = GetComponent<EnemyDestructable>();
        if (!_destructable)
        {
            Debug.LogWarning($"{name}: Missing EnemyDestructable component.");
        }

        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        NoiseManager.OnNoiseMade += OnNoiseHeard;
        if (_destructable != null)
        {
            _destructable.OnHostileDamageTaken += OnDamageTakenHandler;
        }
    }

    private void OnDisable()
    {
        NoiseManager.OnNoiseMade -= OnNoiseHeard;
        if (_destructable != null)
        {
            _destructable.OnHostileDamageTaken -= OnDamageTakenHandler;
        }
    }

    private void Start()
    {
        _player = FindFirstObjectByType<Player>().gameObject;
        SetState(new IdleState(this));
        InitializeDetectionVisuals();
    }

    private void Update()
    {
        _currentState?.Update();
        UpdateDetectionVisuals();
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

        // Calculate direction and distance to the player
        Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        // Check if the player is within the rear detection range
        if (distanceToPlayer <= _rearDetectionRange)
        {
            return HasLineOfSight(); // Use line of sight for rear detection
        }

        // Check if the player is within the FOV range and angle
        if (distanceToPlayer <= _fovRange)
        {
            // Calculate the angle between the enemy's forward direction and the player
            float angleToPlayer = Vector3.Angle(transform.up, directionToPlayer);

            // Check if the player is within the FOV cone
            if (angleToPlayer <= _fovAngle / 2f)
            {
                return HasLineOfSight(); // Use line of sight for FOV detection
            }
        }

        return false; // Player is outside the detection range
    }



    public virtual bool HasLineOfSight()
    {
        if (_player == null) return false;

        Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        // Perform a raycast to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, LayerMask.GetMask("Obstacle"));
        return hit.collider == null; // True if no obstacle blocks the way
    }

    public void ChasePlayer()
    {
        if (_player != null)
        {
            Agent.SetDestination(_player.transform.position);
            Agent.speed = _chaseSpeed;
            LookInDirection(GetPlayerDirection());
        }
    }

    public void LookInDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _chaseSpeed);
        }
    }

    public Vector3 GetPlayerDirection()
    {
        if (_player == null) return Vector3.zero;
        return (_player.transform.position - transform.position).normalized;
    }

    public virtual void ChooseAttack() { }

    protected virtual void OnNoiseHeard(Vector3 noisePosition, float noiseRadius)
    {
        if (Vector3.Distance(transform.position, noisePosition) <= noiseRadius)
        {
            SetState(new ChaseState(this));
            Agent.SetDestination(noisePosition);
        }
    }

    protected virtual void OnDamageTakenHandler()
    {
        ChooseAttack();
    }
    public bool ReachedDestination()
    {
        // Check if the agent has finished its path and reached its stopping point
        return !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance;
    }
    public Vector3 PickRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _fovRange; // Use FOV range as the maximum distance
        randomDirection += transform.position; // Offset by the enemy's position

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, _fovRange, NavMesh.AllAreas))
        {
            Agent.SetDestination(navHit.position);
            Agent.speed = _normalMoveSpeed;
            return navHit.position;
        }

        return transform.position; // Return the current position if no valid destination found
    }
    private void InitializeDetectionVisuals()
    {
        if (_detectionCirclePrefab != null)
        {
            // Instantiate the detection circle and match its scale to the rear detection range
            _circleInstance = Instantiate(_detectionCirclePrefab, transform.position, Quaternion.identity, transform);
            _circleInstance.transform.localScale = new Vector3(_rearDetectionRange * 2, _rearDetectionRange * 2, 1); // Diameter = range * 2
        }

        if (_detectionConePrefab != null)
        {
            // Instantiate the detection cone and match its scale to the FOV range
            _coneInstance = Instantiate(_detectionConePrefab, transform.position, Quaternion.identity, transform);

            // Set the radius dynamically
            var coneMeshGenerator = _coneInstance.GetComponent<ConeMeshGenerator>();
            if (coneMeshGenerator != null)
            {
                coneMeshGenerator.SetConeRadius(_fovRange); // Dynamically set cone radius to match detection range
            }

            // Align the cone with the enemy's forward direction
            _coneInstance.transform.localRotation = Quaternion.Euler(0, 0, 0); // Default alignment
        }
    }




    private void UpdateDetectionVisuals()
    {
        if (_circleInstance != null)
        {
            _circleInstance.transform.position = transform.position; // Keep the circle centered on the enemy
        }

        if (_coneInstance != null)
        {
            _coneInstance.transform.position = transform.position; // Keep the cone centered on the enemy

            // Align the cone with the enemy's forward direction
            _coneInstance.transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
        }
    }


}
