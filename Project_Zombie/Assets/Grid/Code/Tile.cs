using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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

    private GameObject m_activeHexagon;
    private Material m_activeHexMat;
    private Color m_selectedColor = Color.cyan;
    private string m_materialProperty = "_EmissionColor";
    private float m_flashRate = 5f;

    public void Init()
    {
       // HexDetails = new HexInfo();
        Neighbours = new List<Tile>();
 //       HexDetails.Name = "Placeholder name";
 //       HexDetails.Description = "Placeholder description";
        SetAppearance();

        //uncomment if you wanna see hexagon flash.
        /*
        SetActiveHexagon(transform.GetChild(0).gameObject);
        SetSelectable(true, true);
        */
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

    public void SetActiveHexagon(GameObject activeHexagon)
    {
        m_activeHexagon = activeHexagon;
        m_activeHexMat = activeHexagon.GetComponent<Renderer>().sharedMaterials[0];
    }

    private void SetSelectable(bool selectable, bool flash=false)
    {
        if (m_activeHexagon == null)
            return;

        if (!selectable)
        {
            m_activeHexMat.SetColor(m_materialProperty, Color.black);
            StopCoroutine(FlashColor());
            return;
        }

        if (flash)
            StartCoroutine(FlashColor());
        else
            m_activeHexMat.SetColor(m_materialProperty, m_selectedColor);
    }
    
    void Start()
    {
        Init();
    }

    void OnDisable()
    {
        SetSelectable(false);
    }

    private IEnumerator FlashColor()
    {
        while (true)
        {
            if (m_activeHexMat.GetColor(m_materialProperty) != m_selectedColor)
                yield return StartCoroutine(LerpMatToColor(m_activeHexMat, m_selectedColor));
            else
                yield return StartCoroutine(LerpMatToColor(m_activeHexMat, Color.black));

            yield return null;
        }
    }

    private IEnumerator LerpMatToColor(Material mat, Color toColor)
    {
        float lerpTime = 0f;
        while (mat.GetColor(m_materialProperty) != toColor)
        {
            lerpTime += (Time.deltaTime * m_flashRate);
            var lerp = Color.Lerp(mat.GetColor(m_materialProperty), toColor, lerpTime);
            m_activeHexMat.SetColor(m_materialProperty, lerp);
            yield return null;
        }
    }
}
