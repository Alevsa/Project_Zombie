using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GridEditorController : MonoBehaviour
{
    public Text ModeText;
    public InputField GridSizeText;
    public InputField ElevationText;
//    public InputField NameText;
//    public InputField DescriptionText;
    public Toggle DeployableToggle;
    public GameObject SelectionPanel;
    public void CreateWorld()
    {
        GridManager.instance.WorldSize = int.Parse(GridSizeText.text);
        GridManager.instance.CreateWorld((GridManager.GridType)Enum.Parse(typeof(GridManager.GridType), ModeText.text));
    }

    public void SaveWorld()
    {
        List<HexInfo> mapToSave = new List<HexInfo>();
        foreach (Tile tile in GridManager.instance.World.Values)
        {
            mapToSave.Add(tile.HexDetails);
        }
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream("Test.map", FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, mapToSave.ToArray());
        stream.Close();
    }

    public void LoadWorld()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream("Test.map", FileMode.Open, FileAccess.Read, FileShare.Read);
        HexInfo[] loadedInfo = (HexInfo[])formatter.Deserialize(stream);
        stream.Close();
        GridManager.instance.LoadMap(loadedInfo);
    }

    public void ClampElevationInput()
    {
        ClampInput(MinElevation, MaxElevation, ElevationText);
    }

    public void ClampGridSizeInput()
    {
        ClampInput(MinGridSize, MaxGridSize, GridSizeText);
    }

    public void ClampInput(int aMin, int aMax, InputField aInputField)
    {
        int output;
        if (int.TryParse(aInputField.text, out output))
        {
            if (output > aMax)
            {
                aInputField.text = aMax.ToString();
            }
            else if(output < aMin)
            {
                aInputField.text = aMin.ToString();
            }
            // Else value is ok
        }
    }

    public void IncrementSelectedTileElevation(int aQuantity)
    {
        int newElevation = SelectedTile.HexDetails.Elevation + aQuantity;
        if (newElevation < MinElevation || newElevation > MaxElevation)
        {
        }
        else 
        {
            SelectedTile.HexDetails.Elevation = newElevation;
            SelectedTile.SetAppearance();
        }
    }

    void InitialiseSelectionPanel()
    {
        SelectionPanel.SetActive(true);
        DeployableToggle.isOn = SelectedTile.HexDetails.DeploymentTile;
    }
/*
    public void SetTileName()
    {
        SelectedTile.HexDetails.Name = NameText.text;
    }

    public void SetTileDescription()
    {
    }
*/
    public void SetTileAsDeployable()
    {
        SelectedTile.HexDetails.DeploymentTile = DeployableToggle.isOn;
    }
    public Tile SelectedTile = null;

// From here on out it's not ui controls
// -----------------------------------------------------------------------------------------------------------------------
    public int ZoomSpeed = 25;
    public int MaximumZoom = -12;
    public int PanSpeed = 50;
    public int DragSpeed = 50;
    public int ScrollSpeed = 10;
    public float LeftRightScrollArea = 0.05f;
    public float TopBottomScrollArea = 0.05f;
    public int MaxElevation = 3;
    public int MinElevation = -3;
    public int MaxGridSize = 40;
    public int MinGridSize = 1;
    public Transform SelectionIndicator;

    enum eMouseMode { Select, Elevation }
    eMouseMode ActiveMode;

    void Update()
    {
        MouseCameraMovement();
        SetMode();
        MouseInput();
    }

    void MouseCameraMovement()
    {
        Vector3 translation = new Vector3(0, 0, 0);
        // Move camera with mouse
        if (Input.GetMouseButton(2)) // MMB
        {
            // Hold button and drag camera around
            translation = new Vector3(-Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime
                                    , -Input.GetAxis("Mouse Y") * DragSpeed * Time.deltaTime
                                    , 0
                              );
        }
        else
        {

            // Move camera if mouse pointer reaches screen borders
            if (Input.mousePosition.x < Screen.width * LeftRightScrollArea)
            {
                translation += Vector3.left * ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x >= Screen.width - (Screen.width * LeftRightScrollArea))
            {
                translation += Vector3.right * ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y < Screen.height * TopBottomScrollArea)
            {
                translation += Vector3.down * ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y > Screen.height - (Screen.height * TopBottomScrollArea))
            {
                translation += Vector3.up * ScrollSpeed * Time.deltaTime;
            }
        }

        Camera.main.transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime));
        if (Camera.main.transform.position.z > MaximumZoom)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, MaximumZoom);
        Camera.main.transform.Translate(translation);
    }

    void SetMode()
    {
        if (Input.GetButton("ElevationEditMode"))
        {
            ActiveMode = eMouseMode.Elevation;
        }
        else
        {
            ActiveMode = eMouseMode.Select;
        }
    }

    void MouseInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LeftClick();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RightClick();
        }
    }

    void LeftClick()
    {
        switch (ActiveMode)
        {
            case eMouseMode.Select:
                SelectTile();
                break;
            case eMouseMode.Elevation:
                ElevationClick(1);
                break;
            default:
                break;
        }
    }

    void SelectTile()
    {
        Transform obj = GetClickedObject();
        if (obj != null)
        {
            SelectedTile = obj.GetComponent<Tile>();
            InitialiseSelectionPanel();
        }
        else
        {
        }
        PlaceSelectionIndicator();
    }

    void PlaceSelectionIndicator()
    {
        if (SelectedTile == null)
        {
            SelectionIndicator.gameObject.SetActive(false);
        }
        else
        {
            SelectionIndicator.gameObject.SetActive(true);
            SelectionIndicator.position = SelectedTile.transform.position;
            SelectionIndicator.Translate(new Vector3(0, 0, -3)); // Will need changing
        }
    }

    // Currently can move anywhere without regard for neighbors 
    void RightClick()
    {
        switch (ActiveMode)
        {
            case eMouseMode.Select:
                break;
            case eMouseMode.Elevation:
                ElevationClick(-1);
                break;
            default:
                break;
        }
    }

    void ElevationClick(int aDirection)
    {
        Transform obj = GetClickedObject();
        if (obj != null)
        {
            Tile clickedTile = obj.GetComponent<Tile>();
            clickedTile.HexDetails.Elevation += aDirection;
            if (clickedTile.HexDetails.Elevation > MaxElevation)
                clickedTile.HexDetails.Elevation = MinElevation;
            else if (clickedTile.HexDetails.Elevation < MinElevation)
                clickedTile.HexDetails.Elevation = MaxElevation;
            clickedTile.SetAppearance();
            PlaceSelectionIndicator();
        }
    }

    Transform GetClickedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform;
        }
        return null;
    }
}
