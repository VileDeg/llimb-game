using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    private Vector2 _velocity;

    private Rigidbody2D _rb;

    // A scale for scale :)
    private float _baseScale = 1f;

    [SerializeField]
    private float _maxScale = 2f;

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.localScale *= _baseScale;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void SetScaleByFactor(float factor)
    {
        Debug.Assert(factor <= 1.01f);
        transform.localScale *=
            ( _baseScale * (1f - factor) + _maxScale * factor );
    }

    protected virtual void Move()
    {
        _rb.MovePosition(
            GameUtils.ComputeEulerStep(_rb.position, _velocity, Time.fixedDeltaTime));

        if (GameManager.Instance.EscapedLevel(_rb.position, new(1, 1))) {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            Destroy(this.gameObject);
        }
    }
}
