using UnityEngine;

public struct LevelSurface
{
    public readonly int materialType;
    public readonly Vector3 position;
    public readonly Vector2 size;
    public readonly Quaternion rotation;
    public readonly float area;
    public readonly Vector3 normal;
    public readonly Vector3 tangent;
    public readonly Vector3 bitangent;
    
    public LevelSurface(Transform surface, int materialType = 0)
    {
        this.materialType = materialType;
        position = surface.position;
        size = surface.localScale;
        rotation = surface.rotation;
        area = size.x * size.y;
        normal = rotation * Vector3.forward;
        tangent = rotation * Vector3.right;
        bitangent = rotation * Vector3.up;
    }

    public LevelSurface(Vector3 position, Quaternion roataion, Vector2 size, int materialType = 0)
    {
        this.materialType = materialType;
        this.position = position;
        this.size = size;
        this.rotation = roataion;
        this.area = size.x * size.y;
        this.normal = rotation * Vector3.forward;
        this.tangent = rotation * Vector3.right;
        this.bitangent = rotation * Vector3.up;
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

    public Vector3 GetClosestPosition(Vector3 point)
    {
        Vector3 localPos = Quaternion.Inverse(rotation) * (point - position);
        Vector2 hSize = size / 2;
        float clampedX = Mathf.Clamp(localPos.x, -hSize.x, hSize.x);
        float clampedY = Mathf.Clamp(localPos.y, -hSize.y, hSize.y);
        return rotation * new Vector3(clampedX, clampedY, 0) + position;
    }

    public Matrix4x4 GetTRS() => Matrix4x4.TRS(position, rotation, size);

    public Mesh CreateMesh()
    {
        Matrix4x4 trs = GetTRS();
        Mesh mesh = new Mesh();

        int[] triangles = new int[] { 0, 2, 1, 0, 3, 2 };
        Vector3[] normals = new Vector3[] { normal, normal, normal, normal };

        Vector3[] vertices = new Vector3[] 
        { 
            trs.MultiplyPoint(new Vector3(-.5f, -.5f)),
            trs.MultiplyPoint(new Vector3(-.5f,  .5f)), 
            trs.MultiplyPoint(new Vector3( .5f,  .5f)),
            trs.MultiplyPoint(new Vector3( .5f, -.5f))
        };

        Vector2[] uvs = new Vector2[]
        {
            new Vector2(Vector3.Dot(vertices[0], tangent), Vector3.Dot(vertices[0], bitangent)),
            new Vector2(Vector3.Dot(vertices[1], tangent), Vector3.Dot(vertices[1], bitangent)),
            new Vector2(Vector3.Dot(vertices[2], tangent), Vector3.Dot(vertices[2], bitangent)),
            new Vector2(Vector3.Dot(vertices[3], tangent), Vector3.Dot(vertices[3], bitangent)),
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;

        mesh.RecalculateBounds();
        return mesh;
    }
}