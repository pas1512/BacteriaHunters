using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Room
{
    [SerializeField] private Color _color;
    public Color color => _color;

    [SerializeField] private Vector2 _position;
    public Vector2 position => _position;

    [SerializeField] private Vector2 _size;
    public Vector2 size => _size;

    [SerializeField] private List<Door> _doors;
    public Door[] doors => _doors.ToArray();

    public Room(Vector2 position, Vector2 size)
    {
        _color = new Color(Random.value / 2, Random.value / 2, Random.value / 2);
        _position = position;
        _size = size;
        _doors = new List<Door>();
    }

    public void SetWallWidth(float wallWidth)
    {
        wallWidth /= 2;
        _size -= new Vector2(wallWidth, wallWidth);
    }

    public void SetDoor(Door door)
    {
        _doors.Add(door);
    }
}