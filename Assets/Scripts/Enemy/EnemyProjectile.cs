using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : AProjectile
{
    protected override LayerMask GetCollisionMask()
    {
        return LayerMask.GetMask("Obstacle", "Player");
    }
}
