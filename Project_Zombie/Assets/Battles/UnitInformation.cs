using UnityEngine;
using System.Collections;

public class UnitInformation 
{
    public GameObject Unit;
    public Tile Tile;

    public UnitInformation(GameObject unit, Tile tile)
    {
        Unit = unit;
        Tile = tile;
    }
}
