using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct MoveJob : IJobParallelFor
{
    private readonly static Vector3 DIR1 = new Vector3( 0,  0.000f,  1.0f);
    private readonly static Vector3 DIR2 = new Vector3(0, -0.866f, -0.5f);
    private readonly static Vector3 DIR3 = new Vector3(0, 0.866f, -0.5f);
    private readonly static Vector3 DIR4 = new Vector3(0, -1.000f, 0.0f);

    [ReadOnly] private NativeArray<Vector3> _accelerations;
    [ReadOnly] private NativeArray<LevelSurface> _surfaces;
    
/*    private NativeArray<DemoRay> _casts;
    private NativeArray<DemoRay> _casts1;
    private NativeArray<DemoRay> _casts2;
    private NativeArray<DemoRay> _casts3;*/
    private NativeArray<Bacterium> _data;
    private float _deltaTime;

    public MoveJob(NativeArray<Vector3> accelerations, NativeArray<LevelSurface> surfaces, NativeArray<Bacterium> bacteriaDatas, float deltaTime)
    {
        _accelerations = accelerations;
        _surfaces = surfaces;
        _data = bacteriaDatas;
        _deltaTime = deltaTime;
/*        _casts = default;
        _casts1 = default;
        _casts2 = default;
        _casts3 = default;*/
    }

/*    public void SetCustst(NativeArray<DemoRay> c1, NativeArray<DemoRay> c2, NativeArray<DemoRay> c3, NativeArray<DemoRay> c4)
    {
        _casts = c1;
        _casts1 = c2;
        _casts2 = c3;
        _casts3 = c4;
    }*/

    private RaycastResult SearchClossestHit(Vector3 point)
    {
        float min = float.MaxValue;
        Vector3 closest = Vector3.zero;
        Vector3 normal = Vector3.zero;

        for (int i = 0; i < _surfaces.Length; i++)
        {
            Vector3 cl = _surfaces[i].GetClosestPosition(point);
            float sqrMag = Vector3.SqrMagnitude(cl - point);

            if (sqrMag < min)
            {
                min = sqrMag;
                closest = cl;
                normal = _surfaces[i].normal;
            }
        }

        return new RaycastResult(0, closest, normal);
    }

    public void Execute(int index)
    {
        var data = _data[index];
        float minDistance = float.MaxValue;
        RaycastResult clossestHit = default;
        Vector3 raycastOrigin = data.position + (data.normal * data.size * 0.4f);
        float castSize = data.size + data.speed * 0.01f;

        Quaternion rot = Quaternion.LookRotation(data.velocity, data.normal);
        Vector3 d1 = rot * DIR1 * castSize;
        Vector3 d2 = rot * DIR2 * castSize;
        Vector3 d3 = rot * DIR3 * castSize;
        Vector3 d4 = rot * DIR4 * castSize;

        bool inAir = true;

        for (int s = 0; s < _surfaces.Length; s++)
        {
            Vector3 rayOrigin = raycastOrigin;

            if(_surfaces[s].Raycast(rayOrigin, d1, castSize, out var hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
                inAir = false;
            }

            //_casts[index] = new DemoRay(rayOrigin, d1);
            rayOrigin += d1;

            if (_surfaces[s].Raycast(rayOrigin, d2, castSize, out hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
                inAir = false;
            }

            //_casts1[index] = new DemoRay(rayOrigin, d2);
            rayOrigin += d2;

            if (_surfaces[s].Raycast(rayOrigin, d3, castSize, out hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
                inAir = false;
            }

            //_casts2[index] = new DemoRay(rayOrigin, d3);
            rayOrigin += d3;

            if (_surfaces[s].Raycast(rayOrigin, d4, castSize, out hit) &&
                hit.distance < minDistance)
            {
                minDistance = hit.distance;
                clossestHit = hit;
                inAir = false;
            }

            //_casts3[index] = new DemoRay(rayOrigin, d4);
        }

        if (inAir) clossestHit = SearchClossestHit(data.position);

        data.UpdateNormal(clossestHit.normal, clossestHit.point);
        data.ApplyAcceleration(_accelerations[index], _deltaTime);
        _data[index] = data;
    }
}