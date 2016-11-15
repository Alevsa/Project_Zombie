using UnityEngine;
using System.Collections;

public class TurnOrderState : BattleState {

    // Use this for initialization
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        owner.ui.turnOrderPanel.ShowTurnOrder(TurnOrder.GetTurnOrder(owner.unitList, 4));
        yield return null;
    }
    // Update is called once per frame
    void Update () {
	
	}
}
