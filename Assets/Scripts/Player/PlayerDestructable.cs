using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerDestructable : ADestructable
{
    [SerializeField]
    private HealthBar healthBar;

    protected override void Awake()
    {
        base.Awake();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth((int)GetMaxHealth());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var hostileD = collision.gameObject.GetComponent<HostileDestructor>();
        if (hostileD != null)
        {
            TakeDamage(hostileD.GetDamage());
            LogUtil.Info($"{GetType().Name}: took damage {hostileD.GetDamage()}");
            SpawnBlueCircle(collision.contacts[0].point);

            if (healthBar != null)
            {
                healthBar.SetHealth((int)_currentHealth);
            }

            var phd = collision.gameObject.GetComponent<ProjectileHostileDestructor>();
            if (phd != null)
            {
                // Destroy projectile when it does damage
                Destroy(collision.gameObject);
            }
        }
    }
}
