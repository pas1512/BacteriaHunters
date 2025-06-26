public struct FeedingData
{
    public readonly bool inited;
    public readonly bool isBacterium;
    public readonly bool active;
    public readonly int targetId;
    public readonly float size;
    
    public FeedingData(bool inited, bool isBacterium, bool active, int targetId, float size)
    {
        this.inited = inited;
        this.isBacterium = isBacterium;
        this.active = active;
        this.targetId = targetId;
        this.size = size;
    }
}