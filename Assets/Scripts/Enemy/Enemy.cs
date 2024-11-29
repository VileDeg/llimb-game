using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private float _chaseSpeed = 3f;

    [SerializeField] 
    private float _normalMoveSpeed = 1.5f;

    [SerializeField] 
    private float _detectionRadius = 3f;

    [SerializeField] 
    private float _randomMoveInterval = 2f;

    private Rigidbody _rb;
    private CircleCollider2D _collider;

    private GameObject _player;
    private Vector3 _randomDirection;
    private float _randomMoveTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _player = FindAnyObjectByType<Player>().gameObject;
        _randomMoveTimer = _randomMoveInterval;
        PickRandomDirection();
    }

    private void Update()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= _detectionRadius)
        {
            ChasePlayer();
            _randomMoveTimer = 0;
        }
        else
        {
            RandomMovement();
        }
    }

    private void ChasePlayer()
    {
        Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;
        LookInDirection(directionToPlayer);
        
        Vector2 velocity = directionToPlayer * _chaseSpeed;
        transform.position = GameUtils.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
    }

    private void RandomMovement()
    {
        _randomMoveTimer -= Time.deltaTime;

        if (_randomMoveTimer <= 0 || 
            GameManager.Instance.EscapedLevel(
                transform.position, new(_collider.radius, _collider.radius)))
        {
            PickRandomDirection();
            
            _randomMoveTimer = _randomMoveInterval;
        }
        
        Vector3 velocity = _randomDirection * _normalMoveSpeed;
        transform.position =  GameUtils.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
    }

    private void PickRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        _randomDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        LookInDirection(_randomDirection);
    }
    
    private void LookInDirection(Vector2 direction)
    {
        transform.up = direction;
    }
    
    // For debugging puproses
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
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
