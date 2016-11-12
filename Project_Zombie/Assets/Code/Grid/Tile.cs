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
    public LayerMask TileLayer = 7;

// All related to grid generation
    public int[] CubicCoordinates;
    public bool EdgeTile = false;
    public int Elevation; // Not being set yet.

    public void Init()
    {
        Debug.Log("Starting Tile");
        Neighbours = new List<Tile>();
    }

    // Will have stuff like textures and stuff in later
    public void SetAppearance()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Elevation);
    }
}
