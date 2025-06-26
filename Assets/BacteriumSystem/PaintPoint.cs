using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct PaintPoint
{
    [SerializeField] private int _color;
    public int color => _color;

    [SerializeField] private Vector3 _point;
    public Vector3 point => _point;

    [SerializeField] private float _size;
    public float size  => _size;

    [SerializeField] private float _rotation;
    public float rotation => _rotation;

    public float area => Mathf.PI * size * size;

    public PaintPoint(Vector3 point, int color, float size, float rotation)
    {
        _point = point;
        _color = color;
        _size = size;
        _rotation = rotation;
    }

    public void SetArea(float area) => _size = MathF.Sqrt(area / Mathf.PI);

    public static float GetOverlapValue(PaintPoint p1, PaintPoint p2)
    {
        float distance = Vector3.Distance(p1.point, p2.point);
        float touchDistance = p1.area + p2.area;
        return 1 - Mathf.Clamp01(distance / touchDistance);
    }

    public static PaintPoint GetClosest(PaintPoint from, List<PaintPoint> to, out int id)
    {
        id = -1;
        float minDistance = float.MaxValue;

        for (int i = 0; i < to.Count; i++)
        {
            float distance = Vector3.SqrMagnitude(from.point - to[i].point);

            if (distance < minDistance)
            {
                id = i;
                minDistance = distance;
            }
        }

        return to[id];
    }
}