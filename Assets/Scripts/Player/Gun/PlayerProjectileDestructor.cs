using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileDestructor : ADestructor
{
    [SerializeField]
    protected float _maxDamage = 10.0f;

    public void SetDamageByFactor(float factor)
    {
        _damage = _baseDamage * (1f - factor) + _maxDamage * factor;
    }
}
