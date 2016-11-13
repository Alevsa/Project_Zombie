using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnOrderController : Singleton<TurnOrderController> {

    private Dictionary<GameObject, int> mainUnitList;
    public List<GameObject> turnOrderList;

	// Use this for initialization
	void Start () {
        mainUnitList = new Dictionary<GameObject, int>();
        turnOrderList = new List<GameObject>();

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in units)
            AddUnit(unit);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void AddUnit(GameObject entity)
    {
        if (mainUnitList == null)
            return;

        if (!mainUnitList.ContainsKey(entity))
            mainUnitList.Add(entity, entity.GetComponent<IActive>().initiative);
    }

    public void RemoveUnit(GameObject entity)
    {
        if (mainUnitList.ContainsKey(entity))
            mainUnitList.Remove(entity);
    }

    //Advance to the next turn
    public void AdvanceTurn(Dictionary<GameObject, int> unitList)
    {
        List<GameObject> keys = new List<GameObject>(unitList.Keys);

        foreach (GameObject key in keys)
            unitList[key] += key.GetComponent<IActive>().initiative;
    }

    //Return unit whose turn it is to act
    public GameObject CalculateTurn(Dictionary<GameObject, int> unitList)
    {
        return unitList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
    }
    
    //Call when unit takes an action
    public void ActionTaken(Dictionary<GameObject, int> unitList, GameObject entity)
    {
        unitList[entity] = entity.GetComponent<IActive>().initiative / 2;
    }

    //Get the turn order for the amountOfTurns turns
    public void GetTurnOrder (int amountOfTurns)
    {
        if (mainUnitList == null || mainUnitList.Count == 0)
            return;

        Dictionary<GameObject, int> calcUnitList = new Dictionary<GameObject, int>(mainUnitList);

        for (int i = 0; i < amountOfTurns; i++)
        {
            GameObject entity = CalculateTurn(calcUnitList);

            if (turnOrderList.Count == amountOfTurns)
                turnOrderList.RemoveAt(i);

            turnOrderList.Insert(i, entity);
            ActionTaken(calcUnitList, entity);
            AdvanceTurn(calcUnitList);
        }
    }
}
