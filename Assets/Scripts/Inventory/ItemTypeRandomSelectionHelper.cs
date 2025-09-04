using UnityEngine;

public class ItemTypeRandomSelectionHelper
{
    private ItemType[] _types;
    private float _totalWeight;
    private float[] _prefix;

    public ItemTypeRandomSelectionHelper(ItemType[] surfaces)
    {
        _types = surfaces;
        _prefix = new float[surfaces.Length];

        for (int i = 0; i < surfaces.Length; i++)
        {
            _totalWeight += surfaces[i].spawnChance;
            _prefix[i] = _totalWeight;
        }
    }

    public ItemType GetRandom()
    {
        float randomValue = Random.Range(0, _totalWeight);
        int min = 0;
        int max = _types.Length - 1;

        while (min < max)
        {
            int mid = (max + min) / 2;

            if (randomValue < _prefix[mid])
                max = mid;
            else
                min = mid + 1;
        }

        return _types[min];
    }
}
