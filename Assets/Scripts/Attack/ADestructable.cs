using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADestructable : MonoBehaviour
{
    // Who caused the damage, an enemy or an ally?
    public enum DamageSource
    {
        Ally,
        Hostile
    }

    [SerializeField]
    private float _maxHealth = 5f;

    [SerializeField]
    private GameObject _contactCircle;

    private float _readonly_currentHealth;
    protected float _currentHealth // Change this to protected
    {
        get => _readonly_currentHealth;
        set
        {
            _readonly_currentHealth = value;
            UpdateSpriteColors();
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    protected List<System.Type> _hostileDestructors;

    public event Action OnHostileDamageTaken;
    
    private List<SpriteRenderer> _spriteRenderers;

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    protected void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    protected virtual void TakeDamage(float damage, DamageSource source)
    {
        _currentHealth = Mathf.Max(0f, _currentHealth - damage);
        
        if (source == DamageSource.Hostile) {
            OnHostileDamageTaken?.Invoke();
        }
    }
    
    protected virtual void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, GetMaxHealth());
    }

    protected virtual void Awake()
    {
        _spriteRenderers = new List<SpriteRenderer>(
            GetComponentsInChildren<SpriteRenderer>()
            );
        _currentHealth = _maxHealth;

        _hostileDestructors = GetHostileDestructors();
    }

    protected abstract List<System.Type> GetHostileDestructors();

    
    protected virtual void Die()
    {
        DestroySelf();
    }

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected void ResolveHostileCollision(
        GameObject go,
        System.Action<ADestructor> onHostileDetected)
    {
        ADestructor hostileD = null;
        foreach (var type in _hostileDestructors) {
            hostileD = go.GetComponent(type) as ADestructor;
            if (hostileD != null) {
                break;
            }
        }

        if (hostileD != null) {
            onHostileDetected?.Invoke(hostileD);
        }
    }

    private void UpdateSpriteColors()
    {
        float healthPercentage = _currentHealth / _maxHealth;
        Color color = new(1f, healthPercentage, healthPercentage); // Red color based on health

        foreach (var spriteRenderer in _spriteRenderers) {
            spriteRenderer.color = color;
        }
    }

    protected void SpawnCircle(Vector3 position, Color color)
    {
        // Spawn a circle at the specified position
        if (_contactCircle != null) {
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
    }
}
