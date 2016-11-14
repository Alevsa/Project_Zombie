using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnOrderUnitDisplay : MonoBehaviour {
    public int ImageNumber;


    private Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (TurnOrderController.instance.turnOrderList.Count > (ImageNumber - 1))
        {
            image.sprite = TurnOrderController.instance.turnOrderList[ImageNumber - 1].GetComponent<UnitInfo>().portrait;
        }
	}
}
