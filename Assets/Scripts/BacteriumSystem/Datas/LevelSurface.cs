using UnityEngine;

public struct LevelSurface
{
    public readonly Vector3 position;
    public readonly Vector2 size;
    public readonly Quaternion rotation;
    public Vector3 normal => rotation * Vector3.forward;
    
    public LevelSurface(Transform surface)
    {
        position = surface.position;
        size = surface.localScale;
        rotation = surface.rotation;
    }

    public LevelSurface(Vector3 position, Quaternion roataion, Vector2 size)
    {
        this.position = position;
        this.size = size;
        this.rotation = roataion;
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

    public Matrix4x4 GetTRS() => Matrix4x4.TRS(position, rotation, size);

    public Mesh GetMesh()
    {
        Matrix4x4 trs = GetTRS();
        Mesh mesh = new Mesh();

        int[] triangles = new int[] { 0, 2, 1, 0, 3, 2 };
        Vector2[] uvs = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        Vector3[] normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
        
        Vector3[] vertices = new Vector3[] 
        { 
            trs.MultiplyPoint(new Vector3(-.5f, -.5f)),
            trs.MultiplyPoint(new Vector3(-.5f,  .5f)), 
            trs.MultiplyPoint(new Vector3( .5f,  .5f)),
            trs.MultiplyPoint(new Vector3( .5f, -.5f))
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;

        mesh.RecalculateBounds();
        return mesh;
    }
}