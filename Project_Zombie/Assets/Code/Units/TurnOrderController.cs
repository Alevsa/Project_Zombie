using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnOrderController : Singleton<TurnOrderController> {

    private Dictionary<GameObject, int> unitList;
    public List<GameObject> turnOrderList;

	// Use this for initialization
	void Start () {
        unitList = new Dictionary<GameObject, int>();
        turnOrderList = new List<GameObject>();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddUnit(GameObject entity)
    {
        if (!unitList.ContainsKey(entity))
            unitList.Add(entity, 0);
    }

    public void RemoveUnit(GameObject entity)
    {
        if (unitList.ContainsKey(entity))
            unitList.Remove(entity);
    }

    //Advance to the next turn
    public void AdvanceTurn(Dictionary<GameObject, int> unitList)
    {
        foreach (GameObject key in unitList.Keys)
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
        if (unitList.Count == 0)
            return;

        Dictionary<GameObject, int> calcUnitList = new Dictionary<GameObject, int>(unitList);

        for (int i = 0; i < amountOfTurns; i++)
        {
            GameObject entity = CalculateTurn(calcUnitList);
            turnOrderList[i] = entity;
            ActionTaken(calcUnitList, entity);
            AdvanceTurn(calcUnitList);
        }
    }
}
