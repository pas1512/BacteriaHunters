using System;
using UnityEngine;

[Serializable]
public struct TargetsSet
{
    [SerializeField] private int _id1;
    [SerializeField] private int _id2;
    [SerializeField] private int _id3;
    [SerializeField] private int _id4;
    [SerializeField] private int _id5;
    [SerializeField] private int _id6;
    [SerializeField] private int _id7;
    [SerializeField] private int _id8;

    public static TargetsSet clear => new TargetsSet() { _id1 = -1, _id2 = -1, _id3 = -1, _id4 = -1, _id5 = -1, _id6 = -1, _id7 = -1, _id8 = -1};

    public bool Contains(int id)
    {
        return id == _id1 || id == _id2 || id == _id3 || id == _id4 || id == _id5 || id == _id6 || id == _id7 || id == _id8;
    }
}