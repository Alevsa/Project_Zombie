using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleState : State
{
    protected BattleController owner;

    public List<GameObject> unitList { get { return owner.unitList; } }
    public Dictionary<int[], Tile> world { get { return owner.world; } }
    public BattleUI ui { get { return owner.ui; } }

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