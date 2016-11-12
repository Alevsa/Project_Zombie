using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnOrderController : Singleton<TurnOrderController> {

    private Dictionary<GameObject, int> unitList;

	// Use this for initialization
	void Start () {
        unitList = new Dictionary<GameObject, int>();
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
    public void AdvanceTurn()
    {
        foreach (GameObject key in unitList.Keys)
            unitList[key] += key.GetComponent<IActive>().initiative;
    }

    //Return unit whose turn it is to act
    public GameObject CalculateTurn()
    {
        return unitList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
    }
    
    //Call when unit takes an action
    public void ActionTaken(GameObject entity)
    {
        unitList[entity] = entity.GetComponent<IActive>().initiative / 2;
    }
}
