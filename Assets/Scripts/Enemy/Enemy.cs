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
    
    protected GameObject _player;
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

    public bool PlayerInDetectionRadius()
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

    public Vector3 GetPlayerDirection()
    {
        return (_player.transform.position - transform.position);
    }

    public virtual void ChooseAttack() {}
}
