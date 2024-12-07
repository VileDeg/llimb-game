using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    private Vector2 _velocity;

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    protected virtual void Move()
    {
        transform.position =
            GameUtils.ComputeEulerStep(transform.position, _velocity, Time.deltaTime);

        if (GameManager.Instance.EscapedLevel(transform.position, new(1, 1))) {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        Move();
    }
}
