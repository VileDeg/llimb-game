using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IAttack))]
public class Player : MonoBehaviour
{
    [SerializeField] 
    private Transform _firePoint;

    [SerializeField] 
    private float _moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _moveVelocity;

    private float _moveVert;
    private float _moveHoriz;

    private IAttack _attack;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _attack = GetComponent<AShooting>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LookAtCursor();
        Move();
    }

    private void Move()
    {
        Vector3 pos = transform.position;

        _moveVelocity = new Vector2(_moveHoriz, _moveVert) * _moveSpeed;

        pos = GameUtils.ComputeEulerStep(pos, _moveVelocity, Time.deltaTime);
        transform.position = pos;
    }

    // Called by Input System
    void OnMoveVertical(InputValue value)
    {
        _moveVert = value.Get<float>();
        //Debug.Log("Got _moveVert : " + _moveVert.ToString());
    }

    // Called by Input System
    void OnMoveHorizontal(InputValue value)
    {
        _moveHoriz = value.Get<float>();
        //Debug.Log("Got _moveHoriz: " + _moveHoriz.ToString());
    }

    void OnAttack()
    {
        Vector3 direction = (GetCursorPos() - transform.position).normalized;
        _attack.Attack(_firePoint.position - transform.position, direction);
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