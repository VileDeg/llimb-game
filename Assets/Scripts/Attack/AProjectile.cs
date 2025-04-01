using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    protected Vector2 _velocity;

    protected Rigidbody2D _rb;

    // A scale for scale :)
    protected float _baseScale = 1f;

    protected LayerMask _collisionMask;

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _collisionMask = GetCollisionMask();
    }

    private void Start()
    {
        transform.localScale *= _baseScale;
    }

    void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        _rb.MovePosition(
            GameUtils.ComputeEulerStep(_rb.position, _velocity, Time.fixedDeltaTime));
    }

    protected abstract LayerMask GetCollisionMask();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_collisionMask.value & (1 << collision.gameObject.layer)) != 0) {
            LogUtil.Info("AProjectile OnCollisionEnter2D, mask pass");
            Destroy(this.gameObject);
        }
    }
}
