using UnityEngine;
using System.Collections;

public class Heuristic 
{
	private Tile m_GoalNode;

	public Heuristic(Tile goalNode)
	{
		m_GoalNode = goalNode;
	}

	//euclidean distance used as the cost value estimate.
	public float Estimate(Tile node)
	{
		return Vector3.Distance (node.transform.position, node.transform.position);
	}
}
