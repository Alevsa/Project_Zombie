using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Tile : MonoBehaviour
{
    public string Name = "Tile";
    public string Description = "A short description of the tile";


    // Feel free to delete these as apprpriate for the movement/pathfinding
    public int MovementPenalty = 0;
    public List<Tile> Neighbours;
//    public List<Unit> Occupants;
    public LayerMask TileLayer = 7;

// All related to grid generation
    public int[] CubicCoordinates;
    public bool EdgeTile = false;
    public int Elevation; // Not being set yet.

    public void Init()
    {
        Debug.Log("Starting Tile");
//        Occupants = new List<Unit>();
//        SimpleOccupantGrab();
        Neighbours = new List<Tile>();
    }
/*
    void SimpleOccupantGrab()
    {
        Occupants = transform.GetComponentsInChildren<Unit>().ToList<Unit>();
    }

    public void Enter(Unit aUnit)
    {
        Occupants.Add(aUnit);
    }

    public void Leave(Unit aUnit)
    {
        Occupants.Remove(aUnit);
    }
    */
}
