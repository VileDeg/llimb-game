using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADestructable : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 5f;

    private float _readonly_currentHealth;
    private float _currentHealth
    {
        get => _readonly_currentHealth;
        set {
            _readonly_currentHealth = value;
            //HealthChanged?.Invoke(_readonly_currentHealth / _maxHealth);
            if (_currentHealth <= 0) {
                Die();
            }
        }
    }

    public event Action<float> HealthChanged;

    [SerializeField]
    private GameObject _contactCircle;

    protected virtual void Awake()
    {
        _currentHealth = _maxHealth;
    }

    //protected virtual void OnEnable()
    //{
    //    HealthChanged += OnHealthChanged;
    //}

    //protected virtual void OnDisable()
    //{
    //    HealthChanged -= OnHealthChanged;
    //}


    //public void HealSelf(float heal)
    //{
    //    _currentHealth = Mathf.Min(_maxHealth, _currentHealth + heal);
    //}

    //public float GetDamage()
    //{
    //    return _damage;
    //}

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    protected void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    //protected bool NoHealthLeft()
    //{
    //    return _currentHealth <= 0;
    //}

    protected virtual void Die()
    {
        DestroySelf();
    }

    protected void DestroySelf()
    {
        //if (_explosionPrefab != null) {
        //    Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        //}

        Destroy(gameObject);
    }

    protected void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Max(0f, _currentHealth - damage);
    }

    protected void SpawnCircle(Vector3 position, Color color)
    {
        // Spawn a red circle at the specified position
        if (_contactCircle != null) {
            //position.z = -10;
            var obj = Instantiate(_contactCircle, position, Quaternion.identity);
            if (obj.TryGetComponent<SpriteRenderer>(out var renderer)) {
                renderer.color = color;
                renderer.sortingOrder = 5;
            }
            obj.transform.SetParent(this.transform);
            Destroy(obj, 0.5f);
        }
    }

    protected void SpawnRedCircle(Vector3 position)
    {
        SpawnCircle(position, Color.red);
    }

    protected void SpawnBlueCircle(Vector3 position)
    {
        SpawnCircle(position, Color.blue);
    }




    /* *******************************
     * EVENT CALLBACKS
     * *******************************/


    protected virtual void OnHealthChanged(float health)
    {
        //if (_currentHealth <= 0.0f) {
        //    SelfDestroy();
        //}
    }
}
