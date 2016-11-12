using UnityEngine;
using System.Collections;

public class NodeRecord 
{
	public Tile Node;
	public Connection Connection;
	public float CostSoFar;
	public float EstimatedTotalCost;
	public float Cost;

	public NodeRecord(Tile node)
	{
		Node = node;
	}
}
