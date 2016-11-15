using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Pathfinding : MonoBehaviour 
{
    private Dictionary<UnitInformation, PathInformation> m_movingUnits = new Dictionary<UnitInformation, PathInformation>();

    public void MoveToTarget(UnitInformation moving, UnitInformation target)
    {
        if (m_movingUnits.ContainsKey(moving))
        {
            if (m_movingUnits[moving].Target.Tile != target.Tile)
                m_movingUnits[moving] = new PathInformation(CalculatePath(new Graph(), moving.Tile, target.Tile, new Heuristic(target.Tile, true)), target);
        }
        else
            m_movingUnits.Add(moving, new PathInformation(CalculatePath(new Graph(), moving.Tile, target.Tile, new Heuristic(target.Tile, true)), target));

        foreach(var p in m_movingUnits[moving].Path)
        {
            p.GetFromNode.Node.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        Move(moving, m_movingUnits[moving]);
    }

    public void Clear()
    {
        m_movingUnits.Clear();
    }

    private void Move(UnitInformation moving, PathInformation pathInfo)
    {
        if (pathInfo.Path.Count > 0)
        {
            pathInfo.NextTile = pathInfo.Path[pathInfo.CurrentPosition].GetToNode.Node;
            if (pathInfo.NextTile != pathInfo.Target.Tile)
            {
                moving.Unit.transform.position = pathInfo.NextTile.transform.position;

                moving.Tile = pathInfo.NextTile;
                pathInfo.CurrentPosition--;
            }
        }
    }

    private List<Connection> CalculatePath(Graph graph, Tile start, Tile end, Heuristic heuristic)
    {
        NodeRecord startRecord = new NodeRecord (start);
        startRecord.Connection = null;
        startRecord.CostSoFar = 0f;
        startRecord.EstimatedTotalCost = heuristic.Estimate (start);

        //Open list are nodes still requiring processing, cost etc.
        List<Tile> open = new List<Tile> ();
        List<NodeRecord> openRec = new List<NodeRecord> ();

        //Closed list are nodes that have been processed.
        List<Tile> closed = new List<Tile> ();
        List<NodeRecord> closedRec = new List<NodeRecord> ();
        open.Add (startRecord.Node);
        openRec.Add (startRecord);

        NodeRecord current = null;

        List<Connection> path = new List<Connection> ();

        while (open.Count > 0) 
        {
            current = openRec [0];
            foreach (NodeRecord node in openRec)
                if (node.EstimatedTotalCost < current.EstimatedTotalCost)
                    current = node;

            if (current.Node == end)
                break;

            List<Connection> connections = graph.GetConnections (current);

            foreach (Connection connection in connections) 
            {
                NodeRecord endNodeRecord = connection.GetToNode;
                float endNodeCost = current.CostSoFar + connection.GetCost;
                float endNodeHeuristic = 0f;

                if (closed.Contains (endNodeRecord.Node)) 
                {
                    if (endNodeRecord.CostSoFar <= endNodeCost) 
                        continue;

                    else 
                    {
                        closed.Remove (endNodeRecord.Node);
                        closedRec.Remove (endNodeRecord);
                        endNodeHeuristic = endNodeRecord.Cost - endNodeRecord.CostSoFar;
                    }
                } 

                else if (open.Contains (endNodeRecord.Node)) 
                {
                    if (endNodeRecord.CostSoFar <= endNodeCost) 
                        continue;

                    else
                        endNodeHeuristic = endNodeRecord.Cost - endNodeRecord.CostSoFar;
                } 

                else 
                    endNodeHeuristic = heuristic.Estimate (endNodeRecord.Node);
                
                endNodeRecord.Cost = endNodeCost;
                endNodeRecord.Connection = connection;
                endNodeRecord.EstimatedTotalCost = endNodeCost + endNodeHeuristic;
                
                if (!open.Contains (endNodeRecord.Node)) 
                {
                    open.Add (endNodeRecord.Node);
                    openRec.Add (endNodeRecord);
                }
            }
            
            closedRec.Add (current);
            closed.Add (current.Node);
            openRec.Remove (current);
            open.Add (current.Node);
        }

        while (current.Node != start) 
        {
            path.Add (current.Connection);
            current = current.Connection.GetFromNode;
        }
        
        return path;
    }
}