using UnityEngine;
using System.Collections;

public class UnitInformation 
{
    public GameObject Unit;
    public Tile Tile;
    public Vector3 Offset;

    public UnitInformation(GameObject unit, Tile tile)
    {
        Unit = unit;
        Tile = tile;
        Offset = new Vector3(0, 0, unit.GetComponent<MeshRenderer>().bounds.size.z / 2f);
    }
}
