using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PaintsContainer 
{
    [SerializeField] private float _dissolutionSpeed = 0.05f;
    [SerializeField] private List<PaintPoint> _paints;
    private bool _changed;

    public PaintPoint[] paints => _paints.ToArray();

    public PaintsContainer()
    {
        _paints = new List<PaintPoint>();
    }

    public void AddPaint(Vector3 point, Vector3 normal, int color, float size)
    {
        if (_paints.Any(p => Vector3.Distance(point, p.point) < (p.size + size) / 4))
            return;

        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 position = point + normal * 0.001f;
        Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal);
        Quaternion rotation = Quaternion.AngleAxis(angle, normal) * normalRotation;
        PaintPoint paint = new PaintPoint(position, rotation, color, size * 2);
        _paints.Add(paint);
        _changed = true;
    }

    public void TakePaint(int id, float size)
    {
        if (id < 0 || id >= _paints.Count)
            return;

        var paint = _paints[id];
        float newArea = paint.GetArea() - size;
        paint.SetArea(newArea);
        _paints[id] = paint;
        _changed = true;
    }

    public void Update(float dt)
    {
        for (int i = 0; i < _paints.Count; i++)
        {
            var paint = _paints[i];
            float size = paint.GetArea() - _dissolutionSpeed * dt;

            if(size > 0.01f)
            {
                paint.SetArea(size);
                _paints[i] = paint;
            }
            else
            {
                _paints.RemoveAt(i);
            }
        }

        _changed = true;
    }

    public void ClearFinished()
    {
        for (int i = _paints.Count - 1; i >= 0; i--)
        {
            if (_paints[i].size <= 0.01f)
                _paints.RemoveAt(i);
        }
    }

    public bool IsChanged()
    {
        bool current = _changed;
        _changed = false;
        return current;
    }
}