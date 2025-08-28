using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct NewMoveJob : IJobParallelFor
{
    private readonly static Vector3 DIR1 = new Vector3(0, 0.000f, 1.0f);
    private readonly static Vector3 DIR2 = new Vector3(0, -0.866f, -0.5f);
    private readonly static Vector3 DIR3 = new Vector3(0, 0.866f, -0.5f);
    private readonly static Vector3 DIR4 = new Vector3(0, -1.000f, 0.0f);

    [ReadOnly] private NativeArray<Vector3> _accelerations;
    [ReadOnly] private NativeArray<LevelSurface> _surfaces;
    private NativeArray<Bacterium> _data;
    private float _deltaTime;

    public void Execute(int index)
    {
        float minDistance = float.MaxValue;
        RaycastResult clossestHit = default;
        Vector3 raycastOrigin = _data[index].position + _data[index].normal * _data[index].size;
        float castSize = _data[index].size + _data[index].speed * 0.01f;

        Quaternion rot = _data[index].rotation;
        Vector3 d1 = rot * DIR1 * castSize;
        Vector3 d2 = rot * DIR2 * castSize;
        Vector3 d3 = rot * DIR3 * castSize;
        Vector3 d4 = rot * DIR4 * castSize;

        for (int s = 0; s < _surfaces.Length; s++)
        {
            Vector3 rayOrigin = raycastOrigin;

            if (_surfaces[s].Raycast(rayOrigin, d1, castSize, out var hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
            }

            rayOrigin += d1;

            if (_surfaces[s].Raycast(rayOrigin, d2, castSize, out hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
            }

            rayOrigin += d2;

            if (_surfaces[s].Raycast(rayOrigin, d3, castSize, out hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
            }

            rayOrigin += d3;

            if (_surfaces[s].Raycast(rayOrigin, d4, castSize, out hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
            }
        }

        var data = _data[index];
        data.UpdateNormal(clossestHit.normal, clossestHit.point);
        data.ApplyAcceleration(_accelerations[index], _deltaTime);
        _data[index] = data;
    }
}
