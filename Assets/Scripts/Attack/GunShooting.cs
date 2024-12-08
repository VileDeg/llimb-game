using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : AShooting
{
    public override void Attack(Vector3 direction, Vector3 firePointOffset, float charge)
    {
        var bulletGO = Instantiate(
            _projectilePrefab,
            transform.position + firePointOffset,
            Quaternion.identity);

        var projectile = bulletGO.GetComponent<AProjectile>();
        Debug.Assert(projectile != null);

        projectile.SetVelocity(direction * _projectileSpeed);
        projectile.SetScaleByFactor(charge);

        var destr = bulletGO.GetComponent<ADestructor>();
        Debug.Assert(destr != null);

        destr.SetDamageByFactor(charge);
    }
}
