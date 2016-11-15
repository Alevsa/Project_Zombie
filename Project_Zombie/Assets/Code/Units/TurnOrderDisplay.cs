using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class TurnOrderDisplay : MonoBehaviour {

    public List<TurnOrderImageDisplay> turnOrderImages;
	// Use this for initialization
	void Start () {
       // turnOrderImages = GetComponentsInChildren<TurnOrderUnitDisplay>().ToList<TurnOrderUnitDisplay>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ShowTurnOrder(List<GameObject> unitList)
    {
        gameObject.SetActive(true);

        
        foreach (var image in turnOrderImages)
        {
            image.GetComponent<Image>().sprite = unitList[image.ImageNumber - 1].GetComponent<UnitInfo>().portrait;
        }
    }

    public void HideTurnOrder()
    {
        gameObject.SetActive(false);
    }

}
