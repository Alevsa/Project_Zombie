using UnityEngine;

public class GridEditorController : MonoBehaviour
{
    Tile SelectedTile = null;

    public int ZoomSpeed = 25;
    public int MaximumZoom = -12;
    public int PanSpeed = 50;
    public int DragSpeed = 50;
    public int ScrollSpeed = 10;
    public float LeftRightScrollArea = 0.05f;
    public float TopBottomScrollArea = 0.05f;

    public int MaxElevation = 3;

    void Update()
    {
        MouseCameraMovement();
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
        Transform obj = GetClickedObject();
        if (obj != null)
        {
            SelectedTile = obj.GetComponent<Tile>();
        }
    }

    // Currently can move anywhere without regard for neighbors 
    void RightClick()
    {
        Transform obj = GetClickedObject();
        if (obj != null)
        {
            Tile clickedTile = obj.GetComponent<Tile>();
            clickedTile.Elevation++;
            if (clickedTile.Elevation > MaxElevation)
                clickedTile.Elevation = 0;
            clickedTile.SetAppearance();
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
