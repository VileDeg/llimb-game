using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _normalMoveSpeed = 1.5f;
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private float _randomMoveInterval = 2f;

    private GameObject _player;
    private Vector2 _randomDirection;
    private float _randomMoveTimer;

    private Vector2 _pos2d
    {
        get => new(transform.position.x, transform.position.y);
        set => transform.position = new Vector3(value.x, value.y, transform.position.z);
    }

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController>().gameObject;
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
        transform.position = GameUtils.ComputeEulerStep(_pos2d, velocity, Time.deltaTime);
    }

    private void RandomMovement()
    {
        _randomMoveTimer -= Time.deltaTime;

        if (_randomMoveTimer <= 0)
        {
            PickRandomDirection();
            
            _randomMoveTimer = _randomMoveInterval;
        }
        
        Vector2 velocity = _randomDirection * _normalMoveSpeed;
        _pos2d = GameUtils.ComputeEulerStep(_pos2d, velocity, Time.deltaTime);
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

    private Vector2 get2DPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}
