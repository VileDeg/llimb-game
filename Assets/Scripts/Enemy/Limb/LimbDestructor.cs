using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDestructor : ADestructor
{
    public virtual float GetDamage()
    {
        return _damage;
    }
}
