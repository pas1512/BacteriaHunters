using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct MoveJob : IJobParallelForTransform
{
    private const int DIRECTION_COUNTS = 18;
    private readonly static Vector3[] DIRECTIONS =
    {
        new Vector3( 1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3( 0, 1, 0),
        new Vector3( 0,-1, 0),
        new Vector3( 0, 0, 1),
        new Vector3( 0, 0,-1),

        new Vector3( 0.707f,-0.707f, 0),
        new Vector3(-0.707f, 0.707f, 0),
        new Vector3(-0.707f,-0.707f, 0),
        new Vector3( 0.707f, 0.707f, 0),

        new Vector3( 0.707f, 0,-0.707f),
        new Vector3(-0.707f, 0, 0.707f),
        new Vector3(-0.707f, 0,-0.707f),
        new Vector3( 0.707f, 0, 0.707f),

        new Vector3(0, 0.707f,-0.707f),
        new Vector3(0,-0.707f, 0.707f),
        new Vector3(0,-0.707f,-0.707f),
        new Vector3(0, 0.707f, 0.707f)
    };

    [ReadOnly] private NativeArray<Vector3> _accelerations;
    [ReadOnly] private NativeArray<CastableSurface> _surfaces;
    private NativeArray<BacteriumData> _data;
    private float _deltaTime;

    public MoveJob(NativeArray<Vector3> accelerations, NativeArray<CastableSurface> surfaces, NativeArray<BacteriumData> bacteriaDatas, float deltaTime)
    {
        _accelerations = accelerations;
        _surfaces = surfaces;
        _data = bacteriaDatas;
        _deltaTime = deltaTime;
    }
    
    public void Execute(int index, TransformAccess transform)
    {
        float minDistance = float.MaxValue;
        RaycastResult clossestHit = default;
        Vector3 upWard = _data[index].rotation * Vector3.up;
        Vector3 raycastOrigin = _data[index].position + upWard * _data[index].size;

        for (int s = 0; s < _surfaces.Length; s++)
        {
            for (int i = 0; i < DIRECTION_COUNTS; i++)
            {
                if (_surfaces[s].Raycast(raycastOrigin, DIRECTIONS[i], _data[index].lookArea, out var hit) &&
                    hit.distance < minDistance)
                {
                    minDistance = hit.distance;
                    clossestHit = hit;
                }
            }
        }

        var data = _data[index];
        data.UpdateNormal(clossestHit.normal, clossestHit.point);
        data.ApplyAcceleration(_accelerations[index], _deltaTime);
        _data[index] = data;
    }
}