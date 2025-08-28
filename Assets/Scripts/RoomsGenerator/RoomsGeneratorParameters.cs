using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GeneratorParameters")]
public class RoomsGeneratorParameters : ScriptableObject
{
    [SerializeField] private MinMaxPair _bacteries = new MinMaxPair(100, 500);
    [SerializeField] private int _steps = 3;
    [SerializeField] private MinMaxPair _roomWidth = new MinMaxPair(3, 5);
    [SerializeField] private MinMaxPair _roomLength = new MinMaxPair(3, 5);
    [SerializeField] private MinMaxPair _generationsSteps = new MinMaxPair(0.01f, 0.5f);
    [SerializeField] private MinMaxPair _neighborsCount = new MinMaxPair(2, 2);
    [SerializeField] private float _minOverlape = 1;
    [SerializeField] private float _doorWidth = 1.5f;
    [SerializeField] private float _wallWidth = 0.5f;

    public int GetStartSteps() => _steps;
    public float GetGenerationStep() => _generationsSteps.GetRandom();
    public int GetNeighborsCount() => (int)_neighborsCount.GetRandom();
    public Vector2 GetDoorSize() => new Vector2(_doorWidth, _wallWidth);
    public float GetSizeX() => _roomWidth.GetRandom();
    public float GetSizeY() => _roomLength.GetRandom();
    internal int GetBacteriesNumber() => (int)_bacteries.GetRandom();
    public float GetWallWidth() => _wallWidth;

    public Vector2 GetMinOverlape()
    {
        float minOverlape = _minOverlape + _doorWidth;
        return new Vector2(minOverlape, minOverlape);
    }
}