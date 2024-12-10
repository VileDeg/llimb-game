using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IAttack))]
public class Player : MonoBehaviour
{
    private enum GunState { 
        None = -1,
        Charge,
        Cooldown
    }


    [SerializeField]
    private Transform _firePoint;

    [SerializeField]
    private GameObject _gunObject;

    [SerializeField] 
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _chargeRate = 25f;

    [SerializeField]
    private float _cooldownRate = 50f;

    private float _timingFactor = 0.1f;

    // Must always be 1!
    private const float _chargeMax = 1f;

    private const float _cooldownMax = 1f;
    //private float _charge = 0f;

    private float _readonly_charge = 0f;
    private float _Charge
    {
        get => _readonly_charge;
        set {
            _readonly_charge = value;
            UpdateGunVisuals();
        }
    }

    private float _readonly_cooldown = 0f;
    private float _Cooldown
    {
        get => _readonly_cooldown;
        set {
            _readonly_cooldown = value;
            UpdateGunVisuals();
        }
    }

    //private bool _attackHeld = false;

    private Rigidbody2D _rb;
    private Vector2 _moveVelocity;

    private float _moveVert;
    private float _moveHoriz;

    private IAttack _attack;

    private LayerMask _collisionMask;
    public float _collisionOfsset = 0.05f; // Small buffer to prevent sticking

    private SpriteRenderer _gunSprite;

    private GunState _gunState = GunState.None;



    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _attack = GetComponent<AShooting>();

        _gunSprite = _gunObject.GetComponent<SpriteRenderer>();

        _collisionMask = LayerMask.GetMask("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        LookAtCursor();

        switch (_gunState) {
            case GunState.Charge:
                if (_Charge < _chargeMax) {
                    _Charge += _chargeRate * _timingFactor * Time.deltaTime;
                }
                break;
            case GunState.Cooldown:
                _Cooldown += _cooldownRate * _timingFactor * Time.deltaTime;
                if (_Cooldown >= _cooldownMax) {
                    _Cooldown = 0f;
                    // Finish cooldown
                    _gunState = GunState.None;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        AttemptMove();
    }

    private void Move(Vector2 direction, float speed)
    {
        _moveVelocity = direction * speed;

        _rb.MovePosition(
            GameUtils.ComputeEulerStep(_rb.position, _moveVelocity, Time.fixedDeltaTime));
    }

    private void AttemptMove()
    {
        Vector2 direction = new Vector2(_moveHoriz, _moveVert);

        if (direction == Vector2.zero)
            return; // No movement input

        // Calculate the distance to move
        float distance = _moveSpeed * Time.fixedDeltaTime;

        // Perform a Rigidbody2D.Cast to detect collisions in the direction of movement
        List<RaycastHit2D> hits = new(); // Array for storing results
        ContactFilter2D filter = new() { layerMask = _collisionMask };

        int hitCount = _rb.Cast(direction, filter, hits, distance + _collisionOfsset); //

        // Check if there's a collision
        if (hitCount == 0) {
            // No collision, move the player
            //_rb.MovePosition(_rb.position + direction * distance);
            Move(direction, _moveSpeed);
        } else {
            // Collision detected, stop movement or adjust position (optional)
            // Slide along walls by allowing movement parallel to the collision surface

            // Get the first hit information
            RaycastHit2D hit = hits[0];

            // Calculate a new movement direction parallel to the surface of the collision
            Vector2 slideDirection = Vector2.Perpendicular(hit.normal);

            // Check which side of the perpendicular is aligned with the original input direction
            if (Vector2.Dot(slideDirection, direction) < 0) {
                slideDirection = -slideDirection; // Flip if the direction is opposite
            }

            // Calculate the sliding speed multiplier based on the angle
            float slideSpeedMultiplier = Mathf.Abs(Vector2.Dot(direction, slideDirection));

            // Adjust the slide speed dynamically
            float slideSpeed = _moveSpeed * slideSpeedMultiplier;

            Move(slideDirection, slideSpeed);
        }
    }

    // Called by Input System
    void OnMoveVertical(InputValue value)
    {
        _moveVert = value.Get<float>();
    }

    // Called by Input System
    void OnMoveHorizontal(InputValue value)
    {
        _moveHoriz = value.Get<float>();
    }

    void Attack()
    {
        _Charge = Mathf.Min(_Charge, 1f);

        Vector3 direction = (GetCursorPos() - transform.position).normalized;
        _attack.Attack(direction, _firePoint.position - transform.position, _Charge);

        _Charge = 0f;
    }

    void OnAttack()
    {
        // Nothing
    }

    void OnAttackCharge(InputValue value)
    {
        if (Mathf.Approximately(value.Get<float>(), 1f)) { // pressed
            if (_gunState == GunState.None) {
                _gunState = GunState.Charge;
            }
        } else if (Mathf.Approximately(value.Get<float>(), 0f)) { // released
            if (_gunState == GunState.Charge) {
                Attack();
                _gunState = GunState.Cooldown;
            }
        }
    }

    private void UpdateGunVisuals()
    {
        float cp = _Charge / _chargeMax;
        float cld = _Cooldown / _cooldownMax;

        if (_gunState == GunState.Charge) {
            _gunSprite.color = Color.Lerp(Color.white, Color.yellow, cp);
        } else if (_gunState == GunState.Cooldown) {
            _gunSprite.color = Color.Lerp(Color.white, Color.blue, cld);
        } else if (_gunState == GunState.None) {
            _gunSprite.color = Color.white;
        }
    }

    void LookAtCursor()
    {
        Vector2 direction = (GetCursorPos() - transform.position).normalized;
        transform.up = direction;
    }

    Vector3 GetCursorPos()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mp;
    }

}