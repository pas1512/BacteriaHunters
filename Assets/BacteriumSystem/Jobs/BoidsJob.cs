using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct BoidsJob : IJobParallelFor
{
    [ReadOnly] private NativeArray<BacteriumData> _data;
    [ReadOnly] private NativeArray<PaintsData> _paints;
    private NativeArray<Vector3> _accelerations;
    private NativeArray<FeedingData> _feeding;

    public BoidsJob(NativeArray<BacteriumData> data, NativeArray<PaintsData> paints, NativeArray<Vector3> accelerations, NativeArray<FeedingData> feedingDatas)
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

        int id = _data[index].typeId;
        var current = _data[index];
        Vector3 position = current.position;
        int bacteriesData = _data.Length - 1;
        int neighboursCount = 0;

        Vector3 leaksSum = Vector3.zero;
        Vector3 targetOffset = Vector3.zero;
        float minDistance = float.MaxValue;
        int enemiesCount = 0;

        FeedingData feedingData = default;

        for (int i = 0; i < bacteriesData; i++)
        {
            var other = _data[i];

            if (i == index)
                continue;

            Vector3 different = position - other.position;

            if (different.magnitude > current.lookArea) continue;

            float distance = different.magnitude;

            if (distance == 0)  distance = 0.0001f;

            if (id == other.typeId)
            {
                averageSpreade += different / (distance * distance);
                averageVelocity += other.velocity;
                averagePosition += other.position;
                neighboursCount++;
            }
            else
            {
                if (current.enemies.Contains(id))
                {
                    float enemySize = other.size;
                    leaksSum += different * enemySize / (distance * distance);
                    enemiesCount++;

                    if (distance < current.size + enemySize)
                        feedingData = new FeedingData(true, true, true, i, enemySize);
                }

                if (current.targets.Contains(id) && distance < minDistance)
                {
                    targetOffset = -different.normalized;
                    minDistance = distance;
                }
            }

        }

        for (int i = 0; i < _paints.Length; i++)
        {
            var paint = _paints[i];
            Vector3 offset = current.position - paint.position;
            float distance = offset.magnitude;

            if (distance > current.lookArea) continue;
            if (distance <= 0.001f) distance = 0.001f;

            id = paint.colorId;

            if (current.enemies.Contains(id))
            {
                float paintSize = paint.size;
                leaksSum += offset * paint.size / (distance * distance);
                enemiesCount++;

                if(distance < current.size + paintSize)
                {
                    if (current.targets.Contains(paint.colorId))
                        feedingData = new FeedingData(true, false, true, i, paintSize);
                    else if (current.enemies.Contains(paint.colorId))
                        feedingData = new FeedingData(true, false, false, i, paintSize);
                    else continue;
                }
            }

            if (current.targets.Contains(id) && distance < minDistance)
            {
                targetOffset = -offset.normalized;
                minDistance = distance;
            }
        }

        if (neighboursCount == 0)
            return;

        Vector3 cohesion = (averagePosition / neighboursCount) - current.position;
        Vector3 separation = averageSpreade / neighboursCount;
        Vector3 alignment = averageVelocity / neighboursCount;

        Vector3 wh = current.boidsWheights;
        Vector3 boidsAcceleration = (cohesion * wh.x) + (separation * wh.y) + (alignment * wh.z);

        if (enemiesCount > 0) leaksSum /= enemiesCount;

        Vector2 pt = current.pointsWheights;
        Vector3 targetsAcceleration = targetOffset * pt.x + leaksSum * pt.y;

        Vector2 nv = current.navigationWheights;
        _accelerations[index] = boidsAcceleration * nv.x + targetsAcceleration * nv.y;
        _feeding[index] = feedingData;
    }
}