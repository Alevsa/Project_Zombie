using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GridCreated : EventArgs
{
	public Dictionary<int[], Tile> Grid;

    public GridCreated(Dictionary<int[], Tile> grid)
    {
        Grid = grid;
    }
}
