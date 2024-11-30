using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AShooting : MonoBehaviour, IAttack
{
    [SerializeField]
    public GameObject _projectilePrefab;

    [SerializeField]
    public float _projectileSpeed;

    public abstract void Attack(Vector3 attackPointOffset, Vector3 direction);
}
