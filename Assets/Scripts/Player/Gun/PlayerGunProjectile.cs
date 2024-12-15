using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunProjectile : AProjectile
{
    [SerializeField]
    protected float _maxScale = 2f;

    public void SetScaleByFactor(float factor)
    {
        Debug.Assert(factor <= 1.01f);
        transform.localScale *=
            (_baseScale * (1f - factor) + _maxScale * factor);
    }

    protected override LayerMask GetCollisionMask()
    {
        return LayerMask.GetMask("Obstacle", "Enemy");
    }
}
