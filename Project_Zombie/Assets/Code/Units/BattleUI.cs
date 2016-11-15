using UnityEngine;
using System.Collections;

public class BattleUI : MonoBehaviour {

    public TurnOrderDisplay turnOrderPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
