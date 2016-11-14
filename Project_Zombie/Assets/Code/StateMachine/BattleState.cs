using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleState : State
{
    protected BattleController owner;

    public List<UnitInfo> unitList { get { return owner.unitList; } }

    protected virtual void Awake()
    {
        owner = GetComponent<BattleController>();
    }

    protected override void AddListeners()
    {

    }

    protected override void RemoveListeners()
    {

    }

}