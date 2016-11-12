using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

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
        StartCoroutine(MoveAfterGridCreated());
    }

    public IEnumerator MoveAfterGridCreated()
    {
        while (GridManager.instance.GetWorld() == null)
            yield return null;

        var world = GridManager.instance.GetWorld();
        UnitInformation testUnit = new UnitInformation(TestUnit, world.Values.First());
        AddUnit(testUnit);
        UnitInformation target = new UnitInformation(TestTarget, world.Values.Last());
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
        foreach (var unit in Units)
        {
            //place unit on his tile.
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
