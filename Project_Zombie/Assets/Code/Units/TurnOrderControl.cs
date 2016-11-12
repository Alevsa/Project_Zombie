using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnOrderControl : Singleton<TurnOrderControl> {

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

    public GameObject CalculateTurn()
    {
        //Advance unit's turn by initiative
        foreach (GameObject key in unitList.Keys)
            unitList[key]+=key.GetComponent<IActive>().initiative;

        //Return unit whose turn it is to act
        return unitList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
    }
    
}
