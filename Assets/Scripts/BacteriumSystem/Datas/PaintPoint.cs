using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct PaintPoint
{
    [SerializeField] private Vector3 _point;
    public Vector3 point => _point;

    [SerializeField] private float _scale;
    public float size => _scale;

    [SerializeField] private Quaternion _rotation;
    public Quaternion rotation => _rotation;

    [SerializeField] private int _color;
    public int color => _color;

    public PaintPoint(Vector3 point, Quaternion rotation, int color, float size)
    {
        _point = point;
        _color = color;
        _scale = size;
        _rotation = rotation;
    }

    public float GetArea()
    {
        return Mathf.PI * size *size;
    }

    public void SetArea(float area)
    {
        _scale = MathF.Sqrt(area / Mathf.PI);
    }

    public Matrix4x4 GetTRS() => Matrix4x4.TRS(_point, _rotation, Vector3.one * _scale);

    public static float GetOverlapValue(PaintPoint p1, PaintPoint p2)
    {
        float distance = Vector3.Distance(p1.point, p2.point);
        float touchDistance = p1.GetArea() + p2.GetArea();
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