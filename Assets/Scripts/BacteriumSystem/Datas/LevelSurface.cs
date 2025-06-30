using UnityEngine;

public struct LevelSurface
{
    public readonly Vector3 position;
    public readonly Vector2 size;
    public readonly Quaternion rotation;
    
    public LevelSurface(Transform surface)
    {
        position = surface.position;
        size = surface.localScale;
        rotation = surface.rotation;
    }

    public bool Raycast(Vector3 origin, Vector3 dir, float maxDist, out RaycastResult raycastResult)
    {
        Vector3 normal = rotation * Vector3.forward;
        float denom = Vector3.Dot(normal, dir);

        if (Mathf.Abs(denom) < 1e-6f) 
        { 
            raycastResult = default; 
            return false; 
        }

        float d = -Vector3.Dot(normal, position);
        float t = -(Vector3.Dot(normal, origin) + d) / denom;

        if (t < 0 || t > maxDist)
        {
            raycastResult = default;
            return false;
        }

        Vector3 hitPoint = origin + dir * t;
        float distance = t;

        Vector3 local = Quaternion.Inverse(rotation) * (hitPoint - position);

        raycastResult = new RaycastResult(distance, hitPoint, normal);
        return Mathf.Abs(local.x) <= size.x * 0.5f && Mathf.Abs(local.y) <= size.y * 0.5f;
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 offset = Vector3.zero;

        offset.x = size.x * Random.Range(-0.5f, 0.5f);
        offset.y = size.y * Random.Range(-0.5f, 0.5f);
        offset.z = 0;

        return position + rotation * offset;
    }
}