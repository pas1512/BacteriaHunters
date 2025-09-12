using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomsGenerator
{
    public Room[] Generate(Vector2 originPoint, RoomParameters parameters)
    {
        Room firstRect = CreateFirstRoom(originPoint, parameters);
        List<Room> rooms = new List<Room>() { firstRect };
        FillRoomList(firstRect, parameters.GetStartSteps(), parameters, rooms);

        for (int i = 0; i < rooms.Count; i++)
            rooms[i].SetWallWidth(parameters.GetWallWidth());

        return rooms.ToArray();
    }

    private Room CreateFirstRoom(Vector3 originPoint, RoomParameters parameters)
    {
        Vector2 size = new Vector2(parameters.GetSizeX(), parameters.GetSizeY());
        return new Room(originPoint, size);
    }

    private bool TryCreateRoom(Vector2 position, Vector2 size, List<Room> rooms, out Room rect)
    {
        rect = new Room(position, size);

        for (int i = 0; i < rooms.Count; i++)
        {
            Room rect2 = rooms[i];

            if (RectTools.RectsIsOverlaped(position, size, rect2.position, rect2.size))
                return false;
        }

        return true;
    }

    private void FillRoomList(Room room, int steps, RoomParameters parameters, List<Room> rooms)
    {
        List<Room> created = new List<Room>();
        float randomOffset = Random.Range(0f, 1f);
        int randomNeighbors = parameters.GetNeighborsCount();

        for (float i = 0; i < 1; i += parameters.GetGenerationStep())
        {
            float t = Mathf.Repeat(randomOffset + i, 1);
            Vector2 size = new Vector2(parameters.GetSizeX(), parameters.GetSizeY());
            float[] limits = RectTools.GetLimitAngles(room.size, size, parameters.GetMinOverlape());
            float angle = RectTools.LerpPairs(t, limits);
            Vector2 offset = new Vector2(MathF.Sin(angle), MathF.Cos(angle));
            Vector2 position = RectTools.GetRectPose(room.position, room.size, size, offset);

            if (TryCreateRoom(position, size, rooms, out Room createdRoom))
            {
                created.Add(createdRoom);
                rooms.Add(createdRoom);

                var intersections = RectTools.GetIntersectPoint(room.position, room.size, position, size);
                Vector2 doorSize = parameters.GetDoorSize();
                Vector2 center = (intersections.p1 + intersections.p2) / 2;
                Vector2 dirToOriginal = (room.position - center).normalized;
                Vector2 perpToOriginal = Vector2.Perpendicular(dirToOriginal).normalized;
                Vector2 tangent = (intersections.p2 - intersections.p1).normalized * (doorSize.x / 2f);
                tangent *= Mathf.Sign(Vector2.Dot(perpToOriginal, tangent)); 
                Vector2 normal = -Vector2.Perpendicular(tangent).normalized * (doorSize.y / 2f);

                room.SetDoor(new Door(center, normal, tangent));
                createdRoom.SetDoor(new Door(center, -normal, -tangent));
            }

            if (created.Count >= randomNeighbors)
                break;
        }

        steps -= 1;

        if (steps <= 0)
            return;

        for (int i = 0; i < created.Count; i++)
            FillRoomList(created[i], steps, parameters, rooms);
    }
}