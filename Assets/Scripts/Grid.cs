using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    public Node[,,] grid;

    float nodeDiameter;
    public int gridSizeX, gridSizeY, gridSizeZ;

    // Add a public list to store the path
    public List<Node> path;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2
            - Vector3.forward * gridWorldSize.z / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius)
                        + Vector3.up * (y * nodeDiameter + nodeRadius)
                        + Vector3.forward * (z * nodeDiameter + nodeRadius);

                    bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                    Debug.LogWarning(walkable);
                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z);
                }
            }
        }
    }

    // Helper function to retrieve node from world position
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Your existing logic to convert worldPosition to grid node
        // For example, this could be:
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);
        float percentZ = Mathf.Clamp01((worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        // Debugging to check the indices and world position
        Debug.Log($"World Position: {worldPosition} -> Grid Indices: (x: {x}, y: {y}, z: {z})");

        // Check if indices are within bounds
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && z >= 0 && z < gridSizeZ)
        {
            return grid[x, y, z]; // Return the node at the calculated indices
        }
        else
        {
            Debug.LogWarning("Position is outside of grid bounds.");
            return null; // Return null if indices are out of bounds
        }
    }
    
    public Node GetNode(int x, int y, int z)
    {
        return grid[x, y, z];
    }
    
    void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null && path.Contains(n))
                    Gizmos.color = Color.black;

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
    
}
[System.Serializable]
public class Node
{
    public bool walkable;  // Can the node be walked on?
    public Vector3 worldPosition;  // The world position of this node
    public int gridX, gridY, gridZ;  // Coordinates in the grid
    public int gCost;  // Cost from the start node to this node
    public int hCost;  // Heuristic cost to the target node
    public Node parent;  // Parent node (for path retracing)

    public Node(bool _walkable, Vector3 worldPoint, int x, int y, int z)
    {
        walkable = _walkable;
        worldPosition = worldPoint;
        gridX = x;
        gridY = y;
        gridZ = z;
    }

    public int fCost
    {
        get { return gCost + hCost; }  // Total cost
    }
}
