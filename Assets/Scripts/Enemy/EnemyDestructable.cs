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

    [SerializeField]
    private float _basePartsForce = 10f;

    [SerializeField]
    private float _maxPartsForce = 20f;

    [SerializeField]
    private float _partsDamage = 10f;

    // Torque [rad]
    private float _basePartsTorque = 0f;
    private float _maxPartsTorque = 10f;

    private Transform[] _parts;

    protected override void Awake()
    {
        base.Awake();

        _parts =
            _partsRoot.GetComponentsInChildren<Transform>(true)
                .Where(t => t != _partsRoot.transform)
                .ToArray();
    }

    protected override List<System.Type> GetHostileDestructors()
    {
        return new List<System.Type>
        {
            typeof(PlayerProjectileDestructor),
            typeof(PlayerMeleeDestructor),
            typeof(LimbDestructor)
        };
    }

    protected override void Die()
    {
        BlowUp();

        // Destroy self
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
