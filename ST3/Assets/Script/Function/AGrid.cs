using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public List<ANode> path;
    public LayerMask unwalkAbleMask;
    public Vector2 gridWorldSize;
    float nodeRadius = 0.1f;
    public ANode[,] grid;
    public ANode[,] buildGrid;

    float nodeDiameter;
    float buildNodeDiameter;
    int gridSizeX;
    int gridSizeY;

    int buildGridSizeX;
    int buildGridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        buildNodeDiameter = nodeDiameter * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        buildGridSizeX = Mathf.RoundToInt(gridWorldSize.x / buildNodeDiameter) + 1;
        buildGridSizeY = Mathf.RoundToInt(gridWorldSize.y / buildNodeDiameter) + 1;
        CreateGrid();
    }
    void CreateGrid()
    {
        grid = new ANode[gridSizeX, gridSizeY];
        buildGrid = new ANode[buildGridSizeX, buildGridSizeY];
        
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x
         / 2 - Vector3.forward * gridWorldSize.y / 2;
        Vector3 worldPoint;
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkAbleMask);
                grid[x, y] = new ANode(walkable, worldPoint, x, y);
            }
        }
        for(int x = 0; x < buildGridSizeX; x++)
        {
            for(int y = 0; y < buildGridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * buildNodeDiameter + nodeRadius * 2) + Vector3.forward * (y * buildNodeDiameter + nodeRadius * 2);
                bool buildAble = !Physics.CheckSphere(worldPoint, nodeRadius * 3, unwalkAbleMask);
                buildGrid[x,y] = new ANode(buildAble, worldPoint, x, y);
            }
        }
    }

    public List<ANode> GetNeighbours(ANode node)
    {
        List<ANode> neighbours = new List<ANode>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX,checkY]);
                }
            }
        }
        return neighbours;
    }
    public ANode GetNodeWalkWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    public ANode GetNodeBuildWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((buildGridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((buildGridSizeY - 1) * percentY);
        return buildGrid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid != null)
        {
            foreach(ANode n in grid)
            {
                Gizmos.color = n.isWalkAble ? Color.white : Color.green;
                if(path != null)
                    if(path.Contains(n))
                        Gizmos.color = Color.black;
                        
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
            foreach(ANode n in buildGrid)
            {
                Gizmos.color = n.isWalkAble ? Color.white : Color.green;
                if(path != null)
                    if(path.Contains(n))
                        Gizmos.color = Color.black;
                        
                Gizmos.DrawCube(n.worldPos + Vector3.up, Vector3.one * (nodeDiameter - 0.1f) * 2);
            }
        }
    }
}
