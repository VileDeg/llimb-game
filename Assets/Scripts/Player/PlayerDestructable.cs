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
        }

        foreach (ContactPoint2D contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
        }
    }
}
