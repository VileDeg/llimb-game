using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _normalMoveSpeed = 1.5f;
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private float _randomMoveInterval = 2f;

    public NavMeshAgent Agent { get; private set; }
    public float DetectionRadius => _detectionRadius;
    public float RandomMoveInterval => _randomMoveInterval;

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

    public void PickRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _detectionRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, _detectionRadius, NavMesh.AllAreas))
        {
            Agent.SetDestination(navHit.position);
            Agent.speed = _normalMoveSpeed;
        }
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
            LookAtPlayer(_player.transform.position);
        }
    }
    private void LookAtPlayer(Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction); // Only rotate around the Z-axis
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _chaseSpeed); // Smooth rotation
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
