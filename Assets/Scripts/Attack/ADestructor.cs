using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADestructor : MonoBehaviour
{
    [SerializeField]
    private float _baseDamage = 5.0f;

    [SerializeField]
    private float _maxDamage = 10.0f;

    private float _damage = 5f;

    private void Awake()
    {
        _damage = _baseDamage;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetDamageByFactor(float factor)
    {
        _damage = _baseDamage * (1f - factor) + _maxDamage * factor;
    }
}
