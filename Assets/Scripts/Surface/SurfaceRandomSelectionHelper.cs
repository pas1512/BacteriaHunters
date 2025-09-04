using UnityEngine;

public class SurfaceRandomSelectionHelper
{
    private LevelSurface[] _surfaces;
    private float _totalWeight;
    private float[] _prefix;

    public SurfaceRandomSelectionHelper(LevelSurface[] surfaces)
    {
        _surfaces = surfaces;
        _prefix = new float[surfaces.Length];

        for (int i = 0; i < surfaces.Length; i++)
        {
            _totalWeight += surfaces[i].area;
            _prefix[i] = _totalWeight;
        }
    }

    public LevelSurface GetRandom()
    {
        float randomValue = Random.Range(0, _totalWeight);
        int min = 0;
        int max = _surfaces.Length - 1;

        while (min < max)
        {
            int mid = (max + min) / 2;

            if(randomValue < _prefix[mid])
                max = mid;
            else
                min = mid + 1;
        }

        return _surfaces[min];
    }
}