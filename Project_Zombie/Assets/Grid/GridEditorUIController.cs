using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GridEditorUIController : MonoBehaviour
{
    public Text ModeText;
    public InputField WorldSizeText;
    public void CreateWorld()
    {
        Debug.Log(WorldSizeText.text);
        GridManager.instance.WorldSize = int.Parse(WorldSizeText.text);
        GridManager.instance.CreateWorld((GridManager.GridType)Enum.Parse(typeof(GridManager.GridType), ModeText.text));
    }

    public void SaveWorld()
    {
        Debug.Log("Saving World (Unimplemented)");
    }

    public void LoadWorld()
    {
        Debug.Log("Loading World (Unimplemented)");
    }

    public void SetWorldSizeToDefault()
    {
        int output;
        bool isInt = int.TryParse(WorldSizeText.text, out output);
        if (isInt && output > 0)
        {
        }
        else
        {
            WorldSizeText.text = "1";
        }
    }
}
