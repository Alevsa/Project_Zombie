using UnityEngine;
using System.Collections;
using System.Linq;

public class InitBattleState : BattleState {

    public override void Enter()
    {
        base.Enter();
        GridManager.instance.CreateWorld(GridManager.GridType.Square);
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        while (GridManager.instance.GetWorld() == null)
            yield return null;
        PlaceUnits();
        yield return null;
    }

    private void PlaceUnits()
    {
        var world = GridManager.instance.GetWorld();

        foreach (UnitInfo unit in owner.unitList)
        {
            System.Random rand = new System.Random();
            unit.tile = world.ElementAt(rand.Next(0, world.Count)).Value;

            unit.transform.position = unit.tile.transform.position;
        }


    }
}
