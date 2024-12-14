using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyDestructable : ADestructable
{
    [SerializeField]
    private GameObject _partsRoot;

    void OnCollisionEnter2D(Collision2D collision)
    {
        var allyD = collision.gameObject.GetComponent<AllyDestructor>();
        if (allyD != null ) {
            TakeDamage(allyD.GetDamage());
            LogUtil.Info($"{GetType().Name}: took damage {allyD.GetDamage()}");

            SpawnRedCircle(collision.contacts[0].point);

            var pad = collision.gameObject.GetComponent<ProjectileAllyDestructor>();
            if (pad != null) {
                // Dest proj when it does dmg
                Destroy(collision.gameObject);
            }
        }
    }

    protected override void Die()
    {
        _partsRoot.SetActive(true);

        Transform[] parts = 
            _partsRoot.GetComponentsInChildren<Transform>(true)
                .Where(t => t != _partsRoot.transform)
                .ToArray();

        // Unparent all children of _partsRoot
        foreach (Transform child in parts) {
            LogUtil.Info("  Unparent part and apply force");

            // Apply force to scatter parts
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            if (rb != null) {
                Vector2 forceDirection = (child.position - _partsRoot.transform.position).normalized;
                rb.AddForce(forceDirection * Random.Range(5f, 10f), ForceMode2D.Impulse);

                // Apply random torque to make parts rotate
                float randomTorque = Random.Range(-10f, 10f);
                rb.AddTorque(randomTorque, ForceMode2D.Impulse);
            }

            child.SetParent(null);
        }

        // Destroy self
        base.Die();
    }
}
