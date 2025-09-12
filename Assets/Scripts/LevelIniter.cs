using UnityEngine;

public class LevelIniter : MonoBehaviour
{
    public const string LEVELS_PATH = "Scriptables/Levels";

    [SerializeField] private RoomMesher _mesher;
    [SerializeField] private LevelMesh _mesh;
    [SerializeField] private GameWorld _world;

    private static Reward _revard;
    public static Reward reward => _revard;

    private void Start()
    {
        int currentLevelNumber = InteractionMenuActions.GetLevelNumber();
        LevelParameters[] levelParameters = Resources.LoadAll<LevelParameters>(LEVELS_PATH);
        LevelParameters level = levelParameters[currentLevelNumber % levelParameters.Length];

        RoomsGenerator generator = new RoomsGenerator();
        Room[] rooms = generator.Generate(transform.position, level.room);
        LevelSurface[] surfaces = _mesher.GetSurfaces(rooms);
        Mesh mesh = RoomMesher.GenerateMesh2(surfaces);
        _mesh.SetMesh(mesh, level.GetMaterials());

        _revard = level.GetReavard();
        _world.SetBacteria(level.GetBacteriaCount(), level.bacterium);
        _world.SetSurfaces(surfaces);
    }
}