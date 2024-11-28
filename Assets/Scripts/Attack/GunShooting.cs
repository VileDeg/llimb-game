using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : AShooting
{
    public override void Shoot(Vector3 firePointOffset, Vector3 direction)
    {
        var bulletGO = Instantiate(
            _bulletPrefab,
            transform.position + firePointOffset,
            Quaternion.identity);

        var projectile = bulletGO.GetComponent<IProjectile>();
        Debug.Assert(projectile != null);

        projectile.Set(new(direction * _bulletSpeed));
    }
}
