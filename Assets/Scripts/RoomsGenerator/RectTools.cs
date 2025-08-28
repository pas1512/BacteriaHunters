using UnityEngine;

public static class RectTools
{
    public static Vector2 GetRectPose(Vector2 rect1Pose, Vector2 rect1Size, Vector2 rect2Size, Vector2 offset)
    {
        Vector2 criticalDirection = rect1Size + rect2Size;
        Vector2 perpendicularDirection = Vector3.Cross(criticalDirection.normalized, Vector3.forward).normalized;

        float offsetX = Mathf.Abs(offset.x);
        float signX = Mathf.Sign(offset.x);

        float offsetY = Mathf.Abs(offset.y);
        float signY = Mathf.Sign(offset.y);

        float dot = Vector2.Dot(perpendicularDirection, new Vector2(offsetX, offsetY));

        if (dot >= 0)
        {
            float db = rect1Size.x + rect2Size.x;
            float dt = db * (offsetY / offsetX);
            return rect1Pose + new Vector2(signX * db, signY * dt);
        }
        else
        {
            float db = rect1Size.y + rect2Size.y;
            float dt = db * (offsetX / offsetY);
            return rect1Pose + new Vector2(signX * dt, signY * db);
        }
    }

    public static (Vector2 p1, Vector2 p2) GetIntersectPoint(Vector2 rect1Pose, Vector2 rect1Size, Vector2 rect2Pose, Vector2 rect2Size)
    {
        Vector2 offset = rect2Pose - rect1Pose;
        float upperYOffset = (rect2Pose.y - rect2Size.y) - (rect1Pose.y + rect1Size.y);
        float bottomYOffset = (rect2Pose.y + rect2Size.y) - (rect1Pose.y - rect1Size.y);

        if (Mathf.Abs(upperYOffset) <= 0.01f ||
            Mathf.Abs(bottomYOffset) <= 0.01f)
        {
            float y = rect1Pose.y + Mathf.Sign(offset.y) * rect1Size.y;
            float x1 = Mathf.Max(rect1Pose.x - rect1Size.x, rect2Pose.x - rect2Size.x);
            float x2 = Mathf.Min(rect1Pose.x + rect1Size.x, rect2Pose.x + rect2Size.x);
            return (new Vector2(x1, y), new Vector2(x2, y));
        }
        else
        {
            float x = rect1Pose.x + Mathf.Sign(offset.x) * rect1Size.x;
            float y1 = Mathf.Max(rect1Pose.y - rect1Size.y, rect2Pose.y - rect2Size.y);
            float y2 = Mathf.Min(rect1Pose.y + rect1Size.y, rect2Pose.y + rect2Size.y);
            return (new Vector2(x, y1), new Vector2(x, y2));
        }
    }

    public static bool RectsIsOverlaped(Vector2 rect1Pose, Vector2 rect1Size, Vector2 rect2Pose, Vector2 rect2Size)
    {
        Vector2 offset = rect2Pose - rect1Pose;

        return Mathf.Abs(offset.x) < Mathf.Abs(rect1Size.x) + Mathf.Abs(rect2Size.x) &&
               Mathf.Abs(offset.y) < Mathf.Abs(rect1Size.y) + Mathf.Abs(rect2Size.y);
    }

    public static float[] GetLimitAngles(Vector2 size1, Vector2 size2, Vector2 padding)
    {
        float vertX = size1.x + size2.x;
        float vertY = size1.y + size2.y - padding.y;
        float vertAngle = Mathf.Atan2(vertY, vertX);

        float horX = size1.x + size2.x - padding.x;
        float horY = size1.y + size2.y;
        float horAngle = Mathf.Atan2(horY, horX);

        return new float[]
        {
            1.571f - vertAngle, 1.571f + vertAngle,
            1.571f + horAngle, 4.712f - horAngle,
            4.712f - vertAngle, 4.712f + vertAngle,
            4.712f + horAngle, 7.854f - horAngle,  
        };
    }

    public static float LerpPairs(float t, params float[] values)
    {
        t = Mathf.Clamp(t, 0, 0.99999f);
        float stepValue = values.Length * 0.5f * t;
        int pair = Mathf.FloorToInt(stepValue);
        float tPair = stepValue - pair;
        return Mathf.Lerp(values[pair * 2], values[pair * 2 + 1], tPair);
    }

    //reserve
    public static Vector3 LerpLoop(float t, params Vector3[] points)
    {
        float stepValue = (points.Length - 1) * t;
        int point = Mathf.FloorToInt(stepValue);
        float blendValue = stepValue - point;
        return Vector3.Lerp(points[point], points[point + 1], blendValue);
    }
}