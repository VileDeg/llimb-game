using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : AShooting
{
    public override void Shoot(Vector3 firePointOffset, Vector3 direction)
    {
        var bulletGO = Instantiate(
            _projectilePrefab,
            transform.position + firePointOffset,
            Quaternion.identity);

        var projectile = bulletGO.GetComponent<AProjectile>();
        Debug.Assert(projectile != null);

        projectile.SetVelocity(direction * _projectileSpeed);
    }
}
