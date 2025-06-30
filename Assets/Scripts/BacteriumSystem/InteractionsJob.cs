using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct InteractionsJob : IJobParallelFor
{
    [ReadOnly] private NativeArray<Bacterium> _data;
    [ReadOnly] private NativeArray<PaintPoint> _paints;
    private NativeArray<Vector3> _accelerations;
    private NativeArray<CastingData> _feeding;

    public InteractionsJob(NativeArray<Bacterium> data, NativeArray<PaintPoint> paints, NativeArray<Vector3> accelerations, NativeArray<CastingData> feedingDatas)
    {
        _data = data;
        _accelerations = accelerations;
        _paints = paints;
        _feeding = feedingDatas;
    }

    public void Execute(int index)
    {
        Vector3 averageSpreade = Vector3.zero;
        Vector3 averageVelocity = Vector3.zero;
        Vector3 averagePosition = Vector3.zero;

        int typeId = _data[index].typeId;
        var current = _data[index];
        Vector3 position = current.position;
        int bacteriesData = _data.Length - 1;
        int neighboursCount = 0;

        Vector3 leaksSum = Vector3.zero;
        Vector3 targetOffset = Vector3.zero;
        float minDistance = float.MaxValue;
        int enemiesCount = 0;

        CastingData feedingData = default;

        for (int i = 0; i < bacteriesData; i++)
        {
            var other = _data[i];

            if (i == index)
                continue;

            Vector3 different = position - other.position;

            if (different.magnitude > current.lookArea) continue;

            float distance = different.magnitude;

            if (distance == 0)  distance = 0.0001f;

            int otherTypeId = other.typeId;

            if (typeId == otherTypeId)
            {
                averageSpreade += different / (distance * distance);
                averageVelocity += other.velocity;
                averagePosition += other.position;
                neighboursCount++;
            }
            else
            {
                if (current.enemies.Contains(otherTypeId))
                {
                    leaksSum += different * other.size / (distance * distance);
                    enemiesCount++;
                }

                if (current.targets.Contains(otherTypeId))
                {
                    targetOffset = -different.normalized;
                    minDistance = distance;

                    if (distance < (current.size + other.size) / 2)
                        feedingData = new CastingData(true, true, false, i);
                }
            }

        }

        for (int i = 0; i < _paints.Length; i++)
        {
            var paint = _paints[i];
            Vector3 offset = current.position - paint.point;
            float distance = offset.magnitude;

            if (distance > current.lookArea) continue;
            if (distance <= 0.001f) distance = 0.001f;

            int colorId = paint.color;

            if (current.enemies.Contains(colorId))
            {
                float paintSize = paint.size;
                leaksSum += offset * paint.size / (distance * distance);
                enemiesCount++;

                if(distance < (current.size + paintSize) / 2)
                {
                    if (current.targets.Contains(paint.color))
                        feedingData = new CastingData(true, false, false, i);
                    else if (current.enemies.Contains(paint.color))
                        feedingData = new CastingData(true, false, true, i);
                    else continue;
                }
            }

            if (current.targets.Contains(colorId) && distance < minDistance)
            {
                targetOffset = -offset.normalized;
                minDistance = distance;
            }
        }

        Vector3 boidsAcceleration = Vector3.zero;

        if (neighboursCount != 0)
        {
            Vector3 cohesion = (averagePosition / neighboursCount) - current.position;
            Vector3 separation = averageSpreade / neighboursCount;
            Vector3 alignment = averageVelocity / neighboursCount;

            Vector3 wh = current.boidsWheights;
            boidsAcceleration = (cohesion * wh.x) + (separation * wh.y) + (alignment * wh.z);
        }
        
        if (enemiesCount > 0) 
            leaksSum /= enemiesCount;

        Vector2 pt = current.pointsWheights;
        Vector3 targetsAcceleration = targetOffset * pt.x + leaksSum * pt.y;

        Vector2 nv = current.navigationWheights;
        _accelerations[index] = boidsAcceleration * nv.x + targetsAcceleration * nv.y;
        _feeding[index] = feedingData;
    }
}