[System.Serializable]
public class TreeDoodad : BaseDoodad
{
    public override bool Pathable { get { return false; } }
    public override string ModelId { get { return "Tree"; } }
}
