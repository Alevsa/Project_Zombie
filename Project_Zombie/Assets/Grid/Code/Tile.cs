using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Tile : MonoBehaviour
{
// Feel free to delete these as apprpriate for the movement/pathfinding
    public int MovementPenalty = 0;
    public List<Tile> Neighbours;
    public LayerMask TileLayer = 7;
    public HexInfo HexDetails = new HexInfo();
    public bool EdgeTile;
    public bool Pathable = true;
    public int MovementCost = 100;

    public void Init()
    {
       // HexDetails = new HexInfo();
        Neighbours = new List<Tile>();
 //       HexDetails.Name = "Placeholder name";
 //       HexDetails.Description = "Placeholder description";
        SetAppearance();
    }

    // Will have stuff like textures and stuff in later
    public void SetAppearance()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -HexDetails.Elevation);
    }

    void CalculatePathingInfo()
    {
        MovementCost = 100 + HexDetails.Doodad.MovementCost;
        MovementCost = 100 + HexDetails.Type.MovementCost;
        Pathable = HexDetails.Doodad.Pathable || HexDetails.Type.Pathable;
    }
}
