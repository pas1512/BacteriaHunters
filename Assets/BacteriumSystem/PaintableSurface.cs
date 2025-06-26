using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PaintableSurface : MonoBehaviour
{
    private const int MAX_BUFFER_SIZE = 512;
    private const string POINTS_COUNT = "_PaintCount";
    private const string PALETTE_FIELD = "_PaletteArray";
    private const string DATA_FIELD = "_Data";
    private const string VECTOR_FIELD = "_Vectors";

    [SerializeField] private Renderer _renderer;
    [SerializeField] private List<PaintPoint> _painted = new List<PaintPoint>();
    [SerializeField] private Color[] _palette;

    public PaintPoint[] paints => _painted.ToArray();

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.sharedMaterial = new Material(_renderer.sharedMaterial);
        _renderer.sharedMaterial.SetColorArray(PALETTE_FIELD, _palette);
    }

    public void AddHit(Vector3 hitPoint, int color, float size, float rotation)
    {
        if (_painted == null)
            _painted = new List<PaintPoint>() { new PaintPoint(hitPoint, color, size, rotation) };
        else if (_painted.All(p => Vector3.Distance(p.point, hitPoint) > size * 0.5f))
            _painted.Add(new PaintPoint(hitPoint, color, size, rotation));

        UpdateMaterial();
    }

    public void TakePaint(int paintId, float size)
    {
        if (paintId >= _painted.Count || paintId < 0)
            return;

        float area = _painted[paintId].area - size;

        if(area <= 0)
        {
            _painted.RemoveAt(paintId);
        }
        else
        {
            var paint = _painted[paintId];
            paint.SetArea(area);
            _painted[paintId] = paint;
        }


        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (_renderer == null)
            return;

        Vector4[] vectors = new Vector4[MAX_BUFFER_SIZE];
        Vector4[] data = new Vector4[MAX_BUFFER_SIZE];
        int size = Math.Min(_painted.Count, MAX_BUFFER_SIZE - 1);

        for (int i = 0; i < size; i++)
        {
            var p = _painted[i];
            vectors[i] = new Vector4(p.point.x, p.point.y, p.point.z, 0);
            data[i] = new Vector4(p.size, p.rotation, p.color, 0);
        }

        var mpb = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(mpb);
        mpb.SetInt(POINTS_COUNT, size);
        mpb.SetVectorArray(VECTOR_FIELD, vectors);
        mpb.SetVectorArray(DATA_FIELD, data);
        _renderer.SetPropertyBlock(mpb);
    }
}