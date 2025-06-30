public struct CastingData
{
    public readonly bool isExist;
    public readonly bool isBacterium;
    public readonly bool selfDestruction;
    public readonly int targetId;
    
    public CastingData(bool isExist, bool isBacterium, bool selfDestruction, int targetId)
    {
        this.isExist = isExist;
        this.isBacterium = isBacterium;
        this.selfDestruction = selfDestruction;
        this.targetId = targetId;
    }
}