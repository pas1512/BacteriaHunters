using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField] private Transform[] _room;
    [SerializeField] private GameWorld _world;

    private void Start()
    {
        LevelSurface[] surfaces = new LevelSurface[_room.Length];

        for (int i = 0; i < surfaces.Length; i++)
            surfaces[i] = new LevelSurface(_room[i]);

        _world.SetSurfaces(surfaces);
    }
}