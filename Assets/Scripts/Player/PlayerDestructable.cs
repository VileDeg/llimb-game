using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerDestructable : ADestructable
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var hostileD = collision.gameObject.GetComponent<HostileDestructor>();
        if (hostileD != null) {
            TakeDamage(hostileD.GetDamage());
            LogUtil.Info($"{GetType().Name}: took damage {hostileD.GetDamage()}");
            SpawnBlueCircle(collision.contacts[0].point);

            var phd = collision.gameObject.GetComponent<ProjectileHostileDestructor>();
            if (phd != null) {
                // Dest proj when it does dmg
                Destroy(collision.gameObject);
            }
        }
    }
}
