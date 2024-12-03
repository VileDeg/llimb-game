using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType type;
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _normalMoveSpeed = 1.5f;
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private float _randomMoveInterval = 2f;
    // Attack - probably create scripts that inherit 90% only attack change
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _rotationSpeed = 500f;
    [SerializeField] private float _rotationAttackInterval = 5f;
    [SerializeField] private float _dashAttackInterval = 2f;
    [SerializeField] private float _rechargeInterval = 3f;
    [SerializeField] private float _shootingInterval = 2f;
    [SerializeField] private float _projectileSpeed = 100;
    // TODO only for Type4
    [SerializeField] private Transform _firePoint;
    
    public NavMeshAgent Agent { get; private set; }
    public EnemyType Type => type;
    public float DetectionRadius => _detectionRadius;
    public float RandomMoveInterval => _randomMoveInterval;
    public float RotationAttackInterval => _rotationAttackInterval;
    public float DashAttackInterval => _dashAttackInterval;
    public float RechargeInterval => _rechargeInterval;
    public float ShootingInterval => _shootingInterval;
    public GameObject ProjectilePrefab => _projectilePrefab;
    public Transform FirePoint => _firePoint;
    
    private GameObject _player;
    private EnemyState _currentState;

    private void Awake()
    {
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

    public bool ReachedDestination()
    {
        return !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance;
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
    
    public void Rotate()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
    
    public void RotateAttack()
    {
        Rotate();

        if (_player != null)
        {
            Agent.Move(GetPlayerDirection() * (_chaseSpeed * Time.deltaTime));
        }
    }

    public void Dash(Vector3 direction)
    {        
        if (_player != null)
        {
            // Move in straight line
            Agent.Move(direction * (_dashSpeed * Time.deltaTime));
        }
    }

    // TODO help the projectile does not move
    public void ShootPlayer()
    {
        var bulletGO = Instantiate(
            _projectilePrefab,
            transform.position + (_firePoint.position - transform.position),
            Quaternion.identity);

        var projectile = bulletGO.GetComponent<AProjectile>();

        projectile.SetVelocity(transform.forward * _projectileSpeed);
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

    public void ChooseAttack()
    {
        switch (type)
        {
            case(EnemyType.Type1): 
                SetState(new DashState(this));
                break;
            case(EnemyType.Type2):
                SetState(new ShootState(this));
                break;
            case(EnemyType.Type3):
                SetState(new RotationAttackState(this));
                break;
            case (EnemyType.Type4):
                SetState(new ChaseState(this));
                break;
            default:
                SetState(new ChaseState(this));
                break;
        }
    }
}
