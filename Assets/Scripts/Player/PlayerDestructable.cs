using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerDestructable : ADestructable
{
    [SerializeField]
    private HealthBar _healthBar;


    protected override void Awake()
    {
        base.Awake();
        if (_healthBar != null)
        {
            _healthBar.SetMaxHealth((int)GetMaxHealth());
        }
    }

    protected override List<System.Type> GetHostileDestructors()
    {
        return new List<System.Type>
        {
            typeof(EnemyDestructor),
            typeof(LimbDestructor),
            typeof(EnemyProjectileDestructor)
        };
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        ResolveHostileCollision(
            collision.gameObject,
            hostileD => {
                TakeDamage(hostileD.GetDamage(), DamageSource.Hostile);
                LogUtil.Info($"{GetType().Name}: took damage {hostileD.GetDamage()}");
                SpawnBlueCircle(collision.contacts[0].point);

                if (_healthBar != null)
                {
                    _healthBar.SetHealth((int)_currentHealth);
                }
            }
        );
        
        // MedPack Management
        var medPack = collision.gameObject.GetComponent<MedPack>();
        if (medPack != null)
        {
            Heal(medPack.GetHealAmount());
            Destroy(collision.gameObject);
        }
    }


    protected override void Heal(float amount)
    {
        base.Heal(amount);
        if (_healthBar != null)
        {
            _healthBar.SetHealth((int)_currentHealth);
        }
        Debug.Log($"{GetType().Name}: healed {amount}, current health: {_currentHealth}");
    }


    protected override void Die()
    {
        base.Die(); // Call base method to destroy the player object
        GameManager.Instance.GameLost(); // Notify the GameManager that the player has died
    }
}
