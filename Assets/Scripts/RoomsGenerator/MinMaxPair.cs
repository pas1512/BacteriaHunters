using System;
using UnityEngine;

[Serializable]
public class MinMaxPair
{
    [SerializeField] private float _min;
    public float min => _min;

    [SerializeField] private float _max;
    public float max => _max;

    public float GetRandom() => UnityEngine.Random.Range(min, max);

    public MinMaxPair() { _min = 0; _max = 1; }

    public MinMaxPair(float min, float max)
    {
        _min = min;
        _max = max;
    }
}