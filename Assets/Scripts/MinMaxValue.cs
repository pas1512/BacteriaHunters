using System;
using UnityEngine;

[Serializable]
public class MinMaxValue
{
    [SerializeField] private float _min;
    public float min => _min;

    [SerializeField] private float _max;
    public float max => _max;

    public float GetRandom() => UnityEngine.Random.Range(min, max);
    public int GetRandomInt() => UnityEngine.Random.Range((int)min, (int)max);

    public MinMaxValue() { _min = 0; _max = 1; }

    public MinMaxValue(float min, float max)
    {
        _min = min;
        _max = max;
    }
}