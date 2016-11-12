using UnityEngine;
using System.Collections;

public class testscript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TurnOrderController.instance.GetTurnOrder(4);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
