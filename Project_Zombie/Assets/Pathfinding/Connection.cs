using UnityEngine;
using System.Collections;

public class Connection 
{
	public NodeRecord GetFromNode { get; private set; }
	public NodeRecord GetToNode { get; private set; }
	public int GetCost { get; private set; }

	public Connection(NodeRecord fromNode, NodeRecord toNode)
	{
		GetFromNode = fromNode;
		GetToNode = toNode;
		GetCost = 1;
	}
}
