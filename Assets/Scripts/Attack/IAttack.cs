using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public void Attack(Vector3 direction, Vector3 attackPointOffset, float charge);
}
