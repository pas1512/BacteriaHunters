using System;
using UnityEngine;

public class LevelIniter : MonoBehaviour
{
    [SerializeField] private int _currentLevel;
    [SerializeField] private RoomsGeneratorParameters[] _parameters;
    [SerializeField] private RoomMesher _mesher;
    [SerializeField] private LevelMesh _mesh;
    [SerializeField] private GameWorld _world;

    private void OnValidate()
    {
        _currentLevel = (int)Mathf.Repeat(_currentLevel, _parameters.Length);
    }

    private void Start()
    {
        var parameters = _parameters[_currentLevel];
        RoomsGenerator generator = new RoomsGenerator();
        Room[] rooms = generator.Generate(transform.position, parameters);
        LevelSurface[] surfaces = _mesher.GetSurfaces(rooms);
        Mesh mesh = RoomMesher.GenerateMesh(surfaces);
        _mesh.SetMesh(mesh);
        _world.SetBacteriasNumber(parameters.GetBacteriesNumber());
        _world.SetSurfaces(surfaces);
    }
}