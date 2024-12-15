using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Collider2D))]
public class EnemyDestructable : ADestructable
{
    [SerializeField]
    private GameObject _partsRoot;

    private Enemy _enemy; // Reference to the Enemy behavior

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<Enemy>(); // Ensure the Enemy script is attached

        // Subscribe to the OnDamageTaken event
        OnDamageTaken += HandleDamageTaken;
    }

    private void HandleDamageTaken(float damage)
    {
        if (_enemy != null)
        {
            // Transition to ChaseState when damaged
            _enemy.SetState(new ChaseState(_enemy));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var allyD = collision.gameObject.GetComponent<AllyDestructor>();
        if (allyD != null)
        {
            TakeDamage(allyD.GetDamage());
            LogUtil.Info($"{GetType().Name}: took damage {allyD.GetDamage()}");

        _parts =
            _partsRoot.GetComponentsInChildren<Transform>(true)
                .Where(t => t != _partsRoot.transform)
                .ToArray();
    }

            var pad = collision.gameObject.GetComponent<ProjectileAllyDestructor>();
            if (pad != null)
            {
                // Destroy the projectile after it deals damage
                Destroy(collision.gameObject);
            }
        }
    }

    protected override void Die()
    {
        if (_partsRoot != null)
        {
            _partsRoot.SetActive(true);

            Transform[] parts =
                _partsRoot.GetComponentsInChildren<Transform>(true)
                    .Where(t => t != _partsRoot.transform)
                    .ToArray();

            // Unparent all children of _partsRoot
            foreach (Transform child in parts)
            {
                LogUtil.Info("  Unparent part and apply force");

                // Apply force to scatter parts
                Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 forceDirection = (child.position - _partsRoot.transform.position).normalized;
                    rb.AddForce(forceDirection * Random.Range(5f, 10f), ForceMode2D.Impulse);

                    // Apply random torque to make parts rotate
                    float randomTorque = Random.Range(-10f, 10f);
                    rb.AddTorque(randomTorque, ForceMode2D.Impulse);
                }

                child.SetParent(null);
            }
        }

        // Destroy the enemy object
        base.Die();
    }

    private void BlowUp()
    {
        _partsRoot.SetActive(true);

        // Unparent all children of _partsRoot
        foreach (Transform child in _parts) {
            //LogUtil.Info("  Unparent part and apply force");

            if (child.TryGetComponent<Rigidbody2D>(out var rb)) {
                // Apply force to scatter parts
                Vector2 forceDirection =
                    (child.position - _partsRoot.transform.position).normalized;
                float force = Random.Range(_basePartsForce, _maxPartsForce);
                rb.AddForce(
                    forceDirection * force, ForceMode2D.Impulse);

                // Apply random torque to make parts rotate
                float torque =
                    Random.Range(_basePartsTorque, _maxPartsTorque)
                    * (Random.value < 0.5f ? -1f : 1f);

                rb.AddTorque(torque, ForceMode2D.Impulse);
            } else {
                LogUtil.Warn("No RB in child limb");
            }

            // Set damage (TODO: move to Awake?)
            if (child.TryGetComponent<LimbDestructor>(out var hd)) {
                hd.SetDamage(_partsDamage);
            } else {
                LogUtil.Warn("No LimbDestructor in child limb");
            }

            child.SetParent(null);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var limbD = collision.gameObject.GetComponent<LimbDestructor>();
        if (limbD) {
            // Enemy's limb is ally to enemy
            TakeDamage(limbD.GetDamage(), DamageSource.Ally);
            LogUtil.Info($"{GetType().Name}: took damage {limbD.GetDamage()}");

            SpawnRedCircle(collision.contacts[0].point);
        }

        ResolveHostileCollision(
            collision.gameObject,
            hostileD => {
                // Player is hostile to enemy
                TakeDamage(hostileD.GetDamage(), DamageSource.Hostile);
                LogUtil.Info($"{GetType().Name}: took damage {hostileD.GetDamage()}");

                SpawnRedCircle(collision.contacts[0].point);
            }
        );
    }
}
