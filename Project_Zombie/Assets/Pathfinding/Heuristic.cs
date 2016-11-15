using UnityEngine;
using System.Collections;

public class Heuristic 
{
	private Tile m_goalNode;
    private bool m_euclidean;

	public Heuristic(Tile goalNode, bool euclidean)
	{
		m_goalNode = goalNode;
        m_euclidean = euclidean;
    }

	public float Estimate(Tile node)
	{
        float est;

        if(m_euclidean)
            est = Vector3.Distance(m_goalNode.transform.position, node.transform.position);
        else
        {
            Vector3 goal = m_goalNode.transform.position;
            Vector3 cur = node.transform.position;

            est = ((Mathf.Abs(goal.x) - Mathf.Abs(cur.x)) + (Mathf.Abs(goal.y) - Mathf.Abs(cur.y)) + (Mathf.Abs(goal.z) - Mathf.Abs(cur.z))) / 2;
        }

        return est;
	}
}
