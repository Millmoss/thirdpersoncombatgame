using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform playerPos;
    Node[,] grid;
    public Vector3 gridSizeTotal;
    public float nodeSize;
    public LayerMask unwalkableMask;
    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;
    public bool displayGridGizmos;

	void Awake ()
    {
        nodeDiameter = nodeSize * 2;
        gridSizeX = Mathf.RoundToInt(gridSizeTotal.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridSizeTotal.z / nodeDiameter);
        MakeGrid();
    }
	
	void Update ()
    {
        if (Mathf.Abs(transform.position.x - playerPos.position.x) > 10f || Mathf.Abs(transform.position.z - playerPos.position.z) > 10f)
            transform.position = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
        MakeGrid();
	}
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSizeTotal);
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .05f));
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x + gridSizeTotal.x / 2) / gridSizeTotal.x;
        float percentZ = (worldPosition.z - transform.position.z + gridSizeTotal.z / 2) / gridSizeTotal.z;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, z];
    }

    public int MaxSize
    {
        get {return gridSizeX * gridSizeZ;}
    }

    void MakeGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];
        Vector3 totalBottomLeft = transform.position - Vector3.right * gridSizeTotal.x / 2 - Vector3.forward * gridSizeTotal.z/2;
        for(int x = 0;x < gridSizeX;x++)
            for(int z = 0;z<gridSizeZ;z++)
            {
                Vector3 worldPoint = totalBottomLeft + Vector3.right * (x * nodeDiameter + nodeSize) + Vector3.forward * (z * nodeDiameter + nodeSize);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeSize,unwalkableMask));
                grid[x, z] = new Node(walkable, worldPoint, x, z);
            }
    }

    public List<Node> GetNeighbors(Node n)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;
                int checkX = n.gridX + x;
                int checkZ = n.gridZ + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbors.Add(grid[checkX, checkZ]);
                }
            }
        }
        return neighbors;
    }
}