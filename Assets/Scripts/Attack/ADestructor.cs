using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADestructor : MonoBehaviour
{
    [SerializeField]
    private float _damage = 5.0f;

    public float GetDamage()
    {
        return _damage;
    }
}
