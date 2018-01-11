using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfind : MonoBehaviour
{
    Grid grid;
    private Vector3[] currentPath;

	void Start ()
    {
		
	}

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public Vector3[] FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        
        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }
                    int newMoveCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newMoveCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMoveCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                        else
                            openSet.UpdateItem(neighbor);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            Vector3[] temp = RetracePath(startNode, targetNode);
            waypoints = new Vector3[temp.Length + 1];
            for (int i = 0; i < temp.Length; i++)
                waypoints[i] = temp[i];
            waypoints[temp.Length] = temp[temp.Length - 1];
        }
        else
            waypoints = new Vector3[0];
        return waypoints;
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector3.zero;
        for (int i = 1;i < path.Count;i++)
        {
            Vector3 directionNew = new Vector3(path[i - 1].gridX - path[i].gridX, 0, path[i - 1].gridZ - path[i].gridZ);
            if (directionNew != directionOld)
                waypoints.Add(path[i].worldPos);
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distZ = Mathf.Abs(a.gridZ - b.gridZ);
        if (distX > distZ)
            return 14 * distZ + 10 * (distX - distZ);
        return 14 * distX + 10 * (distZ - distX);
    }
}
