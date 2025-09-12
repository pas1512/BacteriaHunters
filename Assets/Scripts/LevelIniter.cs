using UnityEngine;

public class LevelIniter : MonoBehaviour
{
    [SerializeField] private RoomsGeneratorParameters[] _parameters;
    [SerializeField] private RoomMesher _mesher;
    [SerializeField] private LevelMesh _mesh;
    [SerializeField] private GameWorld _world;

    private static Reward _revard;
    public static Reward revard => _revard;

    private void Start()
    {
        var parameters = _parameters[InteractionMenuActions.GetLevelNumber() % _parameters.Length];
        RoomsGenerator generator = new RoomsGenerator();
        Room[] rooms = generator.Generate(transform.position, parameters);
        var surfaces = _mesher.GetSurfaces(rooms);
        Mesh mesh = RoomMesher.GenerateMesh2(surfaces);
        _mesh.SetMesh(mesh, parameters.GetLevelMaterials());
        _revard = parameters.GetReavard();
        _world.SetBacteria(parameters.GetBacteriaCount(), parameters.GetBacteriaTypes());
        _world.SetSurfaces(surfaces);
    }
}