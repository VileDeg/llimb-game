using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeProjectile : AProjectile
{
    protected override LayerMask GetCollisionMask()
    {
        return LayerMask.GetMask();
    }
}
