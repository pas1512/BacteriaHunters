using UnityEngine;

public struct RaycastResult
{
    public readonly float distance;
    public readonly Vector3 point;
    public readonly Vector3 normal;

    public RaycastResult(float distance, Vector3 point, Vector3 normal)
    {
        this.distance = distance;
        this.point = point;
        this.normal = normal;
    }
}