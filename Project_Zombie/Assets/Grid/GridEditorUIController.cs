using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GridEditorUIController : MonoBehaviour
{
    public Text ModeText;
    public InputField WorldSizeText;
    public GridEditorController GridEditorController;

    public void CreateWorld()
    {
        Debug.Log(WorldSizeText.text);
        GridManager.instance.WorldSize = int.Parse(WorldSizeText.text);
        GridManager.instance.CreateWorld((GridManager.GridType)Enum.Parse(typeof(GridManager.GridType), ModeText.text));
    }

    public void SaveWorld()
    {
        Debug.Log("Saving World (Unimplemented)");
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
        Debug.Log("Loading World (Unimplemented)");
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream("Test.map", FileMode.Open, FileAccess.Read, FileShare.Read);
        HexInfo[] loadedInfo = (HexInfo[])formatter.Deserialize(stream);
        stream.Close();
        GridManager.instance.LoadMap(loadedInfo);
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
