using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : StateMachine {

    public List<UnitInfo> unitList;


	// Use this for initialization
	void Start () {
        ChangeState<InitBattleState>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
