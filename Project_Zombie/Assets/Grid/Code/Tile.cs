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
    private GameObject mModel;
    private MeshRenderer mMeshRenderer;

    public void Init()
    {
        Neighbours = new List<Tile>();
        if (HexDetails.Type == null)
            HexDetails.Type = new DirtTileType();
        if (HexDetails.Doodad == null )
            HexDetails.Doodad = new EmptyDoodad();
        SetAppearance();
    }

    // Will have stuff like textures and stuff in later
    public void SetAppearance()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -HexDetails.Elevation);
        foreach (Transform t in transform)
        {
            Debug.Log(HexDetails.Type.ModelId);
            Debug.Log(t.name);
            if (t.name == HexDetails.Type.ModelId)
            {
                t.gameObject.SetActive(true);
                mModel = t.gameObject;
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
        mMeshRenderer = mModel.GetComponent<MeshRenderer>();
        mMeshRenderer.materials = new Material[] {
             Resources.Load("3DModels/Materials/" + HexDetails.Type.CapTexture) as Material
            ,Resources.Load("3DModels/Materials/" + HexDetails.Type.SideTexture) as Material
        };
    }

    void CalculatePathingInfo()
    {
        MovementCost += HexDetails.Doodad.MovementCost + HexDetails.Type.MovementCost;
        Pathable = HexDetails.Doodad.Pathable || HexDetails.Type.Pathable;
    }
}
