using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RoomMesherTester : MonoBehaviour
{
    [SerializeField] private RoomMesher _mesher;
    [SerializeField] private Room[] _rooms;
    [SerializeField] private MeshFilter _filter;
    [SerializeField] private MeshCollider _collider;
    [SerializeField, HideInInspector] private LevelSurface[] _levelSurfaces;

    public void SetRooms(Room[] rooms)
    {
        _rooms = rooms;
        var surfaces = _mesher.GetSurfaces(rooms);
        Mesh mesh = RoomMesher.GenerateMesh(surfaces);

        if (_filter == null)
            _filter = GetComponent<MeshFilter>();

        _filter.sharedMesh = mesh;

        if (_collider == null)
            _collider = GetComponent<MeshCollider>();

        _collider.sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        /*if (_mesher == null || _rooms == null || _rooms.Length <= 0)
            return;

        if (_levelSurfaces == null || _levelSurfaces.Length <= 0)
            _levelSurfaces = _mesher.GetSurfaces(_rooms);

        foreach (var s in _levelSurfaces)
            Gizmos.DrawWireMesh(s.GetMesh());*/
    }
}