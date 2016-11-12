using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph 
{
	public List<Connection> GetConnections(NodeRecord current)
	{
        List<Connection> connections = new List<Connection>();
        foreach (var neighbour in current.Node.Neighbours)
            connections.Add(new Connection(current, new NodeRecord(neighbour)));
		
		return connections;
	}
}
