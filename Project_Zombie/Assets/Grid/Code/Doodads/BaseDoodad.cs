[System.Serializable]
public abstract class BaseDoodad
{
    public virtual bool Pathable { get { return true; } }
    public virtual int MovementCost { get { return 100; } }
    public virtual string ModelId { get { return "EmptyDoodad"; } }
}
