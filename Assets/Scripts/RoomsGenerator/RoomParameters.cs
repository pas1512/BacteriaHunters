using System;
using UnityEngine;

[Serializable]
public class RoomParameters
{
    [SerializeField] private int _steps = 3;
    [SerializeField] private MinMaxValue _roomWidth = new MinMaxValue(3, 5);
    [SerializeField] private MinMaxValue _roomLength = new MinMaxValue(3, 5);
    [SerializeField] private MinMaxValue _generationsSteps = new MinMaxValue(0.01f, 0.5f);
    [SerializeField] private MinMaxValue _neighborsCount = new MinMaxValue(2, 2);
    [SerializeField] private float _minOverlape = 1;
    [SerializeField] private float _doorWidth = 1.5f;
    [SerializeField] private float _wallWidth = 0.5f;

    public int GetStartSteps() => _steps;
    public float GetGenerationStep() => _generationsSteps.GetRandom();
    public int GetNeighborsCount() => (int)MathF.Round(_neighborsCount.GetRandom());
    public Vector2 GetDoorSize() => new Vector2(_doorWidth, _wallWidth);
    public float GetSizeX() => _roomWidth.GetRandom();
    public float GetSizeY() => _roomLength.GetRandom();
    public float GetWallWidth() => _wallWidth;

    public Vector2 GetMinOverlape()
    {
        float minOverlape = _minOverlape + _doorWidth;
        return new Vector2(minOverlape, minOverlape);
    }
}