using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

public class BattleManager : Singleton<BattleManager>
{
    public GameObject TestUnit;
    public GameObject TestTarget;

    public List<UnitInformation> Units { get; private set; }
    private Pathfinding m_pathfinding;

    void Start ()
    {
        Units = new List<UnitInformation>();
        m_pathfinding = GetComponent<Pathfinding>();
        GridManager.instance.CreateGrid(GridManager.GridType.Square);
        SetupBattle();
    }

    private void OnGridCreated(object sender, GridCreated e)
    {
        MoveAfterGridCreated();
        EventManager.Remove<GridCreated>(OnGridCreated);
    }

    public void MoveAfterGridCreated()
    {
        UnitInformation testUnit = new UnitInformation(TestUnit, GridManager.instance.Grid.Values.First());
        AddUnit(testUnit);
        UnitInformation target = new UnitInformation(TestTarget,    GridManager.instance.Grid.Values.Last());
        AddUnit(target);
        StartCoroutine(MoveToTarget(testUnit, target));
    }

    public IEnumerator MoveToTarget(UnitInformation unit, UnitInformation target)
    {
        while (unit.Tile != target.Tile)
        {      
            m_pathfinding.MoveToTarget(unit, target);
            yield return new WaitForSeconds(2f);
       
        }
    }


    public void SetupBattle()
    {
        EventManager.Add<GridCreated>(OnGridCreated);
        foreach (var unit in Units)
        {
            //place unit on his tile.
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EventManager.Send<GridCreated>(this, new GridCreated(GridManager.instance.Grid));
        }
    }

    public void ClearBattle()
    {
        ClearUnits();
        m_pathfinding.Clear();
    }

    public void AddUnit(UnitInformation unit)
    {
        Units.Add(unit);
        unit.Unit.transform.position = unit.Tile.transform.position;
    }

    public void AddUnits(List<UnitInformation> units)
    {
        //Grab from object pool?
        //Place on grid
        Units.AddRange(units);
    }

    private void ClearUnits()
    {
        //Deactivate units to object pool.
        Units.Clear();
    }
    
}
