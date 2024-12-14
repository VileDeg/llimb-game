using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADestructor : MonoBehaviour
{
    [SerializeField]
    protected float _baseDamage = 5.0f;

    protected float _damage = 5f;

    protected virtual void Awake()
    {
        _damage = _baseDamage;
    }

    public virtual float GetDamage()
    {
        return _damage;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
