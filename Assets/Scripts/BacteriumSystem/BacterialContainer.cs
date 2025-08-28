using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BacterialContainer
{
    private const float MIN_SURFACE_DISTANCE = 0.01f;

    private List<Bacterium> _bacteria;
    public Bacterium[] bacteria => _bacteria.ToArray();

    private bool _changed;

    public BacterialContainer(int count, BacteriumData[] variants, LevelSurface[] surfaces)
    {
        _bacteria = new List<Bacterium>(count);
        _changed = true;

        for (int i = 0; i < count; i++)
        {
            int randomData = Random.Range(0, variants.Length);
            int randomSurface = Random.Range(0, surfaces.Length);
            LevelSurface surface = surfaces[randomSurface];
            Vector3 normal = surface.normal;
            Vector3 position = surface.GetRandomPosition() + normal * MIN_SURFACE_DISTANCE;
            _bacteria.Add(new Bacterium(variants[randomData], position, normal));
        }
    }

    public void Add(BacteriumData data)
    {
        _bacteria.Add(new Bacterium(data));
        _changed = true;
    }

    public void Add(Bacterium[] bacterium)
    {
        _bacteria.AddRange(bacterium);
        _changed = true;
    }

    public void Remove(int id)
    {
        _bacteria.RemoveAt(id);
        _changed = true;
    }

    public void Remove(int[] ids)
    {
        if (ids == null || ids.Length == 0)
            return;

        for (int i = ids.Length - 1; i >= 0; i--)
            _bacteria.RemoveAt(ids[i]);

        _changed = true;
    }

    public void Transform(int id, Bacterium data)
    {
        _bacteria[id] = _bacteria[id].Transform(data);
    }

    public void SyncData(NativeArray<Bacterium> bacteria)
    {
        for (int i = 0; i < bacteria.Length; i++)
            _bacteria[i] = new Bacterium(bacteria[i]);
    }

    public bool IsChanged()
    {
        bool current = _changed;
        _changed = false;
        return current;
    }
}