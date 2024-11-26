using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float _moveSpeed = 5f;

    [SerializeField] 
    private GameObject _bulletPrefab;

    [SerializeField] 
    private Transform _firePoint;

    private Rigidbody2D _rb;
    private Vector2 _moveVelocity;

    private float _moveVert;
    private float _moveHoriz;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log("Got _moveVert : " + _moveVert.ToString());
    }

    // Called by Input System
    void OnMoveHorizontal(InputValue value)
    {
        _moveHoriz = value.Get<float>();
        Debug.Log("Got _moveHoriz: " + _moveHoriz.ToString());
    }

    void AimAndShoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        transform.up = direction;

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
    }
}