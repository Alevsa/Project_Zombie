using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathInformation
{
    public List<Connection> Path;
    public UnitInformation Target;
    public Tile NextTile;
    public int CurrentPosition;

    public PathInformation(List<Connection> path, UnitInformation target)
    {
        Path = path;
        Target = target;
        NextTile = null;
        CurrentPosition = path.Count - 1;
    }
}
