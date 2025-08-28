using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Door
{
    [SerializeField] private Color _color;
    public Color color => _color;

    [SerializeField] private Vector2 _center;
    public Vector2 center => _center;

    [SerializeField] private Vector2 _normal;
    public Vector2 normal => _normal;

    [SerializeField] private Vector2 _tangent;
    public Vector2 tangent => _tangent;

    public Door(Vector2 center, Vector2 normal, Vector2 tangent)
    {
        _color = new Color(Random.value * 2, Random.value * 2, Random.value * 2);
        _center = center;
        _normal = normal;
        _tangent = tangent;
    }
}