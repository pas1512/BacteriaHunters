using UnityEngine;

public struct PaintsData
{
    public readonly int surfaceId;
    public readonly int paintId;
    public readonly int colorId;
    public readonly float size;
    public readonly Vector3 position;
    public readonly Vector3 surfacePosition;

    public PaintsData(int surfaceId, int paintId, Vector3 surfacePosition, PaintPoint paintPoint)
    {
        this.surfacePosition = surfacePosition;
        this.position = paintPoint.point;
        this.surfaceId = surfaceId;
        this.paintId = paintId;
        this.colorId = paintPoint.color;
        this.size = paintPoint.size;
    }
}