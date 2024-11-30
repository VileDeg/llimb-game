using NUnit.Framework.Constraints;
using UnityEngine;

public static class GameUtils
{
    public static Vector2 ComputeEulerStep(Vector2 f0, Vector2 df0_dt, float delta_t)
    {
        return f0 + delta_t * df0_dt;
    }

    public static Vector2 ComputeSeekAcceleration(Vector2 pos, float _maxAccel, Vector2 targetPos)
    {
        Vector2 dir = targetPos - pos;
        if (dir.magnitude < 1e-3f)
            return Vector2.zero;
        // Seek only if not too close.
        return _maxAccel * (dir / dir.magnitude);
    }

    public static Vector2 ComputeSeekVelocity(
        Vector2 pos, Vector2 _velocity,
        float _maxSpeed, float _maxAccel,
        Vector2 targetPos, float dt
        )
    {
        Vector2 seekAccel = ComputeSeekAcceleration(pos, _maxAccel, targetPos);
        _velocity = ComputeEulerStep(_velocity, seekAccel, dt);
        // And we must also apply the clipping.
        if (_velocity.magnitude > _maxSpeed)
            _velocity *= (_maxSpeed / _velocity.magnitude);
        return _velocity;
    }
}
