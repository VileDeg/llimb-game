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
    public GameObject Player => _player;
    protected GameObject _player;
    protected EnemyState _currentState;

    protected Rigidbody2D _rb;

    private EnemyDestructable _destructable;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _destructable = GetComponent<EnemyDestructable>();
        if (!_destructable) {
            LogUtil.Warn("Enemy _destructable missing");
        }

        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        _destructable.OnHostileDamageTaken += OnDamageTakenHandler;
    }

    private void OnDisable()
    {
        _destructable.OnHostileDamageTaken -= OnDamageTakenHandler;
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
        return Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius;
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
            Agent.SetDestination(_player.transform.position);
            Agent.speed = _chaseSpeed;
            LookInDirection(GetPlayerDirection());
        }
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

    public virtual void ChooseAttack() {}

    protected virtual void OnDamageTakenHandler() {
        // Move to agro state when damaged by player
        ChooseAttack();
    }

}
