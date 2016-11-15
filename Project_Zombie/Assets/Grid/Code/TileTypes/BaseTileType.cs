[System.Serializable]
public abstract class BaseTileType
{
    public virtual bool Pathable { get { return true; } }
    public virtual int MovementCost { get { return 100; } }
    public virtual string ModelId { get { return "BasicHex"; } }
    public abstract string SideTexture { get; }
    public abstract string CapTexture { get; }
}