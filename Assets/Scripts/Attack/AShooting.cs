using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AShooting : MonoBehaviour
{
    [SerializeField]
    public GameObject _projectilePrefab;

    [SerializeField]
    public float _projectileSpeed;

    public abstract void Shoot(Vector3 firePointOffset, Vector3 direction);
}
