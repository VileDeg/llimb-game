using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunShooting : AShooting
{
    public override void Attack(Vector3 direction, Vector3 firePointOffset, float charge)
    {
        var bulletGO = Instantiate(
            _projectilePrefab,
            transform.position + firePointOffset,
            Quaternion.identity);

        var projectile = bulletGO.GetComponent<PlayerGunProjectile>();
        Debug.Assert(projectile != null);

        projectile.SetVelocity(direction * _projectileSpeed);
        projectile.SetScaleByFactor(charge);

        var playerProjD = bulletGO.GetComponent<PlayerProjectileDestructor>();
        Debug.Assert(playerProjD != null);

        playerProjD.SetDamageByFactor(charge);
    }
}
