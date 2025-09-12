using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GeneratorParameters")]
public class RoomsGeneratorParameters : ScriptableObject
{
    [SerializeField] private MinMaxPair _bacteriaCount = new MinMaxPair(100, 500);
    [SerializeField] private BacteriumData[] _bacteriaTypes;
    [SerializeField] private int _steps = 3;
    [SerializeField] private MinMaxPair _roomWidth = new MinMaxPair(3, 5);
    [SerializeField] private MinMaxPair _roomLength = new MinMaxPair(3, 5);
    [SerializeField] private MinMaxPair _generationsSteps = new MinMaxPair(0.01f, 0.5f);
    [SerializeField] private MinMaxPair _neighborsCount = new MinMaxPair(2, 2);
    [SerializeField] private Reward[] _rewards;
    [SerializeField] private float _minOverlape = 1;
    [SerializeField] private float _doorWidth = 1.5f;
    [SerializeField] private float _wallWidth = 0.5f;
    [SerializeField] private Material _floor;
    [SerializeField] private Material _walls;
    [SerializeField] private Material _ceil;

    public int GetStartSteps() => _steps;
    public float GetGenerationStep() => _generationsSteps.GetRandom();
    public int GetNeighborsCount() => (int)MathF.Round(_neighborsCount.GetRandom());
    public Vector2 GetDoorSize() => new Vector2(_doorWidth, _wallWidth);
    public float GetSizeX() => _roomWidth.GetRandom();
    public float GetSizeY() => _roomLength.GetRandom();
    internal int GetBacteriaCount() => (int)MathF.Round(_bacteriaCount.GetRandom());
    public float GetWallWidth() => _wallWidth;
    public BacteriumData[] GetBacteriaTypes() => _bacteriaTypes;

    public Vector2 GetMinOverlape()
    {
        float minOverlape = _minOverlape + _doorWidth;
        return new Vector2(minOverlape, minOverlape);
    }

    public Reward GetReavard()
    {
        Reward selected = _rewards[UnityEngine.Random.Range(0, _rewards.Length)];
        return selected.GetNewReward();
    }

    internal Material[] GetLevelMaterials() => new Material[] { _floor, _walls, _ceil};
}