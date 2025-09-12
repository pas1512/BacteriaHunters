using UnityEngine;

public class UnevenSelector<T> where T : IUnevenSelectable
{
    private T[] _selectables;
    private float _totalWeight;
    private float[] _prefix;

    public UnevenSelector(T[] selectables)
    {
        _selectables = selectables;
        _prefix = new float[selectables.Length];

        for (int i = 0; i < selectables.Length; i++)
        {
            _totalWeight += selectables[i].unevenSelectionWeight;
            _prefix[i] = _totalWeight;
        }
    }

    public T GetRandom()
    {
        float randomValue = Random.Range(0, _totalWeight);
        int min = 0;
        int max = _selectables.Length - 1;

        while (min < max)
        {
            int mid = (max + min) / 2;

            if (randomValue < _prefix[mid])
                max = mid;
            else
                min = mid + 1;
        }

        return _selectables[min];
    }
}
