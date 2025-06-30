using System.Collections.Generic;
using UnityEngine;

public class PaintsContainer 
{
    private List<PaintPoint> _paints;
    private bool _changed;

    public PaintPoint[] paints => _paints.ToArray();

    public PaintsContainer()
    {
        _paints = new List<PaintPoint>();
    }

    public void AddPaint(Vector3 point, Vector3 normal, int color, float size)
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        PaintPoint paint = new PaintPoint(point, Quaternion.AngleAxis(angle, normal), color, size);
        _paints.Add(paint);
        _changed = true;
    }

    public void TryTakePaint(int id, float size)
    {
        if (id < 0 || id >= _paints.Count)
            return;

        var paint = _paints[id];
        float newArea = paint.GetArea() - size;

        if(newArea > 0)
        {
            paint.SetArea(newArea);
            _paints[id] = paint;
        }
        else
        {
            _paints.RemoveAt(id);
        }

        _changed = true;
    }

    public bool IsChanged()
    {
        bool current = _changed;
        _changed = false;
        return current;
    }
}