﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : StateMachine {

    public List<GameObject> unitList;
    public Dictionary<int[], Tile> world;
    public BattleUI ui;


	// Use this for initialization
	void Start () {
        ChangeState<InitBattleState>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
