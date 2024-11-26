using UnityEngine;

public static class GameUtils
{
    public static Vector3 ComputeEulerStep(Vector3 f0, Vector3 df0_dt, float delta_t)
    {
        return f0 + delta_t * df0_dt;
    }

    public static Vector3 ComputeSeekAcceleration(Vector3 pos, float _maxAccel, Vector3 targetPos)
    {
        Vector3 dir = targetPos - pos;
        if (dir.magnitude < 1e-3f)
            return Vector3.zero;
        // Seek only if not too close.
        return _maxAccel * (dir / dir.magnitude);
    }

    public static Vector3 ComputeSeekVelocity(
        Vector3 pos, Vector3 _velocity,
        float _maxSpeed, float _maxAccel,
        Vector3 targetPos, float dt
        )
    {
        Vector3 seekAccel = ComputeSeekAcceleration(pos, _maxAccel, targetPos);
        _velocity = ComputeEulerStep(_velocity, seekAccel, dt);
        // And we must also apply the clipping.
        if (_velocity.magnitude > _maxSpeed)
            _velocity *= (_maxSpeed / _velocity.magnitude);
        return _velocity;
    }
}
