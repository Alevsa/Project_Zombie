using UnityEngine;
using System.Collections;
using System.Linq;

public class InitBattleState : BattleState {

    public override void Enter()
    {
        base.Enter();
        GridManager.instance.CreateGrid(GridManager.GridType.Square);
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        while (GridManager.instance.Grid == null)
            yield return null;

        owner.world = GridManager.instance.Grid;
        PlaceUnits();
        yield return null;
        owner.ChangeState<TurnOrderState>();
    }

    private void PlaceUnits()
    {
        System.Random rand = new System.Random();

        foreach (var unit in owner.unitList)
        {
            unit.GetComponent<IMovable>().tile = owner.world.ElementAt(rand.Next(0, owner.world.Count)).Value;

            Instantiate(unit, unit.GetComponent<IMovable>().tile.transform.position, Quaternion.identity);
        }


    }
}
