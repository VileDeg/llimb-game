using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AShooting : MonoBehaviour
{
    [SerializeField]
    public GameObject _bulletPrefab;

    [SerializeField]
    public float _bulletSpeed;

    public abstract void Shoot(Vector3 firePointOffset, Vector3 direction);
}
