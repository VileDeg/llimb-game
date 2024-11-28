using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IProjectile;

public class PlayerProjectile : MonoBehaviour, IProjectile
{
    SetupData _data;

    public void Set(SetupData data)
    {
        _data = data;
    }

    void Move()
    {
        Vector2 pos = transform.position;
        pos = GameUtils.ComputeEulerStep(pos, _data.velocity, Time.deltaTime);
        transform.position = pos;
    }

    void Update()
    {
        Move();
    }
}
