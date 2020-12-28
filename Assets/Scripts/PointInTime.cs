using UnityEngine;

public class PointInTime
{
    public Vector3 movement;
    public Quaternion rotation;

    public PointInTime(Vector3 _positionDelta, Quaternion _rotation)
    {
        movement = _positionDelta;
        rotation = _rotation;
    }
}
