using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Reward : IUnevenSelectable
{
    [SerializeField] private ItemType _itemType;
    public ItemType itemType => _itemType;

    [SerializeField] private int _minCount = 1;
    [SerializeField] private int _maxCount = 2;
    
    [SerializeField, HideInInspector] private int _count;
    public int count => _count;

    public float unevenSelectionWeight => _itemType.unevenSelectionWeight;

    public Reward(ItemType itemType, int count )
    {
        _itemType = itemType;
        _count = count;
    }

    public Reward GetNewReward()
    {
        return new Reward( _itemType, Random.Range(_minCount, _maxCount + 1));
    }
}
