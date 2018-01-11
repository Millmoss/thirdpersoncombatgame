using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridZ;
    public Node parent;
    int heapIndex;

    public Node(bool w, Vector3 p, int x, int z)
    {
        walkable = w;
        worldPos = p;
        gridX = x;
        gridZ = z;
    }
    
    public int fCost
    {
        get {return gCost + hCost;}
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node n)
    {
        int compare = fCost.CompareTo(n.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(n.hCost);
        }
        return -compare;
    }
}
