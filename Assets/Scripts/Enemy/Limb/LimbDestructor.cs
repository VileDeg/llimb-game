using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDestructor : ADestructor
{
    [SerializeField]
    protected float _velocityDamageMultiplier = 0.1f;

    protected Rigidbody2D _rb;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
    }

    public override float GetDamage()
    {
        // Calculate damage based on the current velocity
        float damage = _damage * _rb.velocity.magnitude * _velocityDamageMultiplier;

        return damage;
    }
}
