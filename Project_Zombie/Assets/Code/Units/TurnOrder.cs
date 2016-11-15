using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class TurnOrder {

    public static GameObject CalculateTurn(List<GameObject> unitList)
    {
        return unitList.OrderByDescending(i => i.GetComponent<IActive>().initiative).FirstOrDefault();
    }

    public static List<GameObject> GetTurnOrder(List<GameObject> unitList, int amountOfTurns)
    {
        if (unitList == null || unitList.Count == 0)
            return null;

        List<GameObject> calcUnitList = new List<GameObject>(unitList);
        List<GameObject> returnList = new List<GameObject>();

        for (int i = 0; i < amountOfTurns; i++)
        {
            GameObject entity = CalculateTurn(calcUnitList);

            returnList.Add(entity);

            ActionTaken(entity);
            AdvanceTurn(calcUnitList);
        }

        return returnList;
    }

    private static void AdvanceTurn (List<GameObject> unitList)
    {
        foreach (var unit in unitList)
            unit.GetComponent<IActive>().initiative += unit.GetComponent<IActive>().initiativeGain;
    }

    private static void ActionTaken (GameObject unit)
    {
        unit.GetComponent<IActive>().initiative = unit.GetComponent<IActive>().initiativeGain / 2;
    }
}