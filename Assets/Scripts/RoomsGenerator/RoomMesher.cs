using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomMesher : MonoBehaviour
{
    [SerializeField] private float _roomHeight = 3.0f;
    [SerializeField] private float _doorHeight = 2.2f;

    private struct PointWrapper
    {
        public readonly Vector3 point;
        public readonly float value;
        public readonly int type;

        public PointWrapper(Vector2 point, Vector2 rectSize, Vector2 rectPose, int type)
        {
            this.point = new Vector3(point.x, 0, point.y);
            this.value = LerpInverse(point, rectSize, rectPose);
            this.type = type;
        }
    }

    public static float LerpInverse(Vector2 point, Vector2 rectSize, Vector2 rectPose)
    {
        Vector2 hSize = rectSize / 2;
        point -= rectPose;
        float xAbs = Mathf.Abs(point.x);
        float yAbs = Mathf.Abs(point.y);
        float xDist = xAbs - hSize.x;
        float yDist = yAbs - hSize.y;
        float perimetr = (rectSize.x + rectSize.y) * 2;
        
        if(Mathf.Abs(xDist - yDist) < 0.001f)
        {
            if (point.x < 0 && point.y > 0)
                return 0;
            else if (point.x > 0 && point.y > 0)
                return rectSize.x / perimetr;
            else if (point.x > 0 && point.y < 0)
                return (rectSize.x + rectSize.y) / perimetr;
            else
                return (rectSize.x + rectSize.x + rectSize.y) / perimetr; 
        }
        else if (yDist < xDist)
        {
            if (point.x > 0)
                return (rectSize.x + hSize.y - point.y) / perimetr;
            else
                return (rectSize.x + rectSize.y + rectSize.x + hSize.y + point.y) / perimetr;
        }
        else
        {
            if (point.y > 0)
                return (hSize.x + point.x) / perimetr;
            else
                return (rectSize.x + rectSize.y + hSize.x - point.x) / perimetr;
        }
    }

    private Vector3 To3D(Vector2 p) => new Vector3(p.x, 0, p.y);

    private PointWrapper[] GetPoints(Room room)
    {
        List<PointWrapper> points = new List<PointWrapper>();
        Vector2 roomSize = room.size * 2;
        Vector2 roomPose = room.position;

        Vector2 angle1 = room.position + (room.size * new Vector2(-1, 1));
        points.Add(new PointWrapper(angle1, roomSize, roomPose, 0));

        Vector2 angle2 = room.position + room.size;
        points.Add(new PointWrapper(angle2, roomSize, roomPose, 0));

        Vector2 angle3 = room.position + (room.size * new Vector2(1, -1));
        points.Add(new PointWrapper(angle3, roomSize, roomPose, 0));

        Vector2 angle4 = room.position + (room.size * new Vector2(-1, -1));
        points.Add(new PointWrapper(angle4, roomSize, roomPose, 0));

        var doors = room.doors;

        if (doors == null || doors.Length < 0)
        {
            points.Add(points[0]);
            return points.ToArray();
        }

        for (int i = 0; i < doors.Length; i++)
        {
            Vector2 origin = doors[i].center + doors[i].normal;

            Vector2 p1 = origin - doors[i].tangent;
            points.Add(new PointWrapper(p1, roomSize, roomPose, 1));

            Vector2 p2 = origin + doors[i].tangent;
            points.Add(new PointWrapper(p2, roomSize, roomPose, 0));
        }

        var ordered = points.OrderBy(p => p.value).ToList();
        ordered.Add(points[0]);
        return ordered.ToArray();
    }

    private LevelSurface GetSurface(PointWrapper p1, PointWrapper p2)
    {
        Vector3 pos1 = p1.point;
        Vector3 pos2 = p2.point;
        Vector3 offset = pos2 - pos1;
        Vector3 center = (pos1 + pos2) / 2f;
        Vector3 surfacePosition = center;
        Vector3 normal = Vector3.Cross(offset, Vector3.down).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, normal);

        if (p1.type == 0)
        {
            Vector2 surfaceSize = new Vector2(offset.magnitude, _roomHeight);
            surfacePosition.y += _roomHeight / 2;
            return new LevelSurface(surfacePosition, rotation, surfaceSize);
        }
        else
        {
            float height = _roomHeight - _doorHeight;
            Vector2 surfaceSize = new Vector2(offset.magnitude, height);
            surfacePosition.y += _doorHeight + (height / 2);
            return new LevelSurface(surfacePosition, rotation, surfaceSize);
        }
    }

    private LevelSurface[] GetDoorPortal(Door door)
    {
        LevelSurface[] result = new LevelSurface[4];
        float width = door.tangent.magnitude * 2;
        float depth = door.normal.magnitude;

        Vector3 normal = To3D(door.normal);
        Vector3 nNormal = normal.normalized;
        Vector3 position1 = To3D(door.center) + (normal / 2);
        Quaternion rotaiton1 = Quaternion.LookRotation(Vector3.up, nNormal);
        Vector2 size1 = new Vector2(width, depth);
        result[0] = new LevelSurface(position1, rotaiton1, size1);

        Vector3 tangent = To3D(door.tangent);
        Vector3 nTangent = tangent.normalized;
        Vector3 center = position1 + (Vector3.up * (_doorHeight * 0.5f));

        Vector3 position2 = center - tangent;
        Vector2 size2 = new Vector2(_doorHeight, depth);
        Quaternion rotation2 = Quaternion.LookRotation(nTangent, nNormal);
        result[1] = new LevelSurface(position2, rotation2, size2);

        Vector3 position3 = center + tangent;
        Quaternion rotation3 = Quaternion.LookRotation(-nTangent, nNormal);
        result[2] = new LevelSurface(position3, rotation3, size2);

        Vector3 position4 = position1 + (Vector3.up * _doorHeight);
        Quaternion rotaiton4 = Quaternion.LookRotation(Vector3.down, nNormal);
        result[3] = new LevelSurface(position4, rotaiton4, size1);

        return result;
    }

    public LevelSurface[] GetSurfaces(Room[] rooms)
    {
        if (rooms == null || rooms.Length <= 0)
            return null;

        List<LevelSurface> surfaces = new List<LevelSurface>();

        for (int r = 0; r < rooms.Length; r++)
        {
            Room room = rooms[r];

            Vector2 size2 = room.size * 2;

            Vector3 position1 = To3D(room.position);
            Quaternion rotation1 = Quaternion.LookRotation(Vector3.up, Vector3.forward);
            surfaces.Add(new LevelSurface( position1, rotation1, size2));

            Vector3 position2 = position1 + (Vector3.up * _roomHeight);
            Quaternion rotation2 = Quaternion.LookRotation(Vector3.down, Vector3.forward);
            surfaces.Add(new LevelSurface(position2, rotation2, size2));

            var points = GetPoints(room);

            for (int i = 1; i < points.Length; i++)
                surfaces.Add(GetSurface(points[i - 1], points[i]));

            var doors = room.doors;

            for (int i = 0; i < doors.Length; i++)
                surfaces.AddRange(GetDoorPortal(doors[i]));
        }

        return surfaces.ToArray();
    }

    public static Mesh GenerateMesh(LevelSurface[] meshes)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        int vertexOffset = 0;

        for (int m = 0; m < meshes.Length; m++)
        {
            Mesh mesh = meshes[m].GetMesh();
            Matrix4x4 transform = Matrix4x4.identity;

            Vector3[] v = mesh.vertices;
            Vector3[] n = mesh.normals;
            Vector2[] uv = mesh.uv;
            int[] t = mesh.triangles;

            // Додаємо вершини
            for (int i = 0; i < v.Length; i++)
            {
                vertices.Add(v[i]);

                if (n.Length > 0)
                    normals.Add(n[i]);
            }

            // Додаємо UV (або заповнюємо нулями)
            if (uv.Length > 0)
                uvs.AddRange(uv);
            else
                for (int i = 0; i < v.Length; i++)
                    uvs.Add(Vector2.zero);

            // Додаємо трикутники зі зсувом
            for (int i = 0; i < t.Length; i++)
                triangles.Add(t[i] + vertexOffset);

            vertexOffset += v.Length;
        }

        // Створюємо фінальний меш
        Mesh combined = new Mesh();
        combined.SetVertices(vertices);
        combined.SetUVs(0, uvs);
        combined.SetTriangles(triangles, 0);
        //combined.SetNormals(normals);
        combined.RecalculateNormals();
        combined.RecalculateBounds();

        return combined;
    }
}