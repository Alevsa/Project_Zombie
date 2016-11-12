using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridEditorUIController : MonoBehaviour
{
    public Text ModeText;
    public Text WorldSizeText;
    public void CreateWorld()
    {
        Debug.Log(WorldSizeText.text);
        GridManager.instance.WorldSize = int.Parse(WorldSizeText.text);
        GridManager.instance.CreateWorld(ModeText.text);
    }

    public void SaveWorld()
    {
    }

    public void LoadWorld()
    {
    }
}
