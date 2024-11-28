using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IProjectile
{
    struct SetupData
    {
        public Vector2 velocity;

        public SetupData(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }

    void Set(SetupData data);
}
