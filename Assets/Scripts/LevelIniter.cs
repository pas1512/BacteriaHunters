using UnityEngine;

public class LevelIniter : MonoBehaviour
{
    [SerializeField] private RoomsGeneratorParameters[] _parameters;
    [SerializeField] private RoomMesher _mesher;
    [SerializeField] private LevelMesh _mesh;
    [SerializeField] private GameWorld _world;

    private void Start()
    {
        var parameters = _parameters[InteractionMenuActions.GetLevelNumber() % _parameters.Length];
        RoomsGenerator generator = new RoomsGenerator();
        Room[] rooms = generator.Generate(transform.position, parameters);
        var surfaces = _mesher.GetSurfaces(rooms);
        Mesh mesh = RoomMesher.GenerateMesh2(surfaces);
        _mesh.SetMesh(mesh, parameters.GetLevelMaterials());
        _world.SetBacteriasNumber(parameters.GetBacteriesNumber());
        _world.SetSurfaces(surfaces);
    }
}