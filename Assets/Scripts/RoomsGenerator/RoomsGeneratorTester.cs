using UnityEngine;

public class RoomsGeneratorTester : MonoBehaviour
{
    [SerializeField] private bool _drawGizmos;
    [SerializeField] private RoomParameters _parameters;
    [SerializeField, HideInInspector] private Room[] _rooms;

    private Vector3 To3D(Vector2 v2) => new Vector3(v2.x, 0, v2.y);
    private Vector2 To2D(Vector3 v2) => new Vector3(v2.x, v2.z);

    [ContextMenu("Regenerate")]
    private void Regenerate()
    {
        if (_parameters == null)
            return;

        RoomsGenerator roomsGenerator = new RoomsGenerator();
        _rooms = roomsGenerator.Generate(To2D(transform.position), _parameters);
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmos || _rooms == null || _rooms.Length <= 0) 
            return;

        for (int i = 0; i < _rooms.Length; i++)
        {
            Room room = _rooms[i];
            Gizmos.color = room.color;
            Gizmos.DrawCube(To3D(room.position), To3D(room.size * 2));
            var doors = room.doors;

            for (int d = 0; d < doors.Length; d++)
            {
                Door door = doors[d];
                Gizmos.color = door.color;
                Vector2 doorSize = door.normal + door.tangent * 2;
                Vector2 doorPosition = door.center + (door.normal * 0.5f);
                Gizmos.DrawCube(To3D(doorPosition), To3D(doorSize));
            }
        }
    }
}