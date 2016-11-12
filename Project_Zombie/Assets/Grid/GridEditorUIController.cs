using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GridEditorUIController : MonoBehaviour
{
    public Text ModeText;
    public Text WorldSizeText;
    public void CreateWorld()
    {
        Debug.Log(WorldSizeText.text);
        GridManager.instance.WorldSize = int.Parse(WorldSizeText.text);
        GridManager.instance.CreateWorld((GridManager.GridType)Enum.Parse(typeof(GridManager.GridType), ModeText.text));
    }

    public void SaveWorld()
    {
    }

    public void LoadWorld()
    {
    }
}
