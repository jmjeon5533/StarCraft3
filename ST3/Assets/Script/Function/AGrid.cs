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

    public Vector2Int BuildGridSize;

    public float BuildNodeDiameter;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;



    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

   
        CreateGrid();
    }
    void CreateGrid()
    {
        var u = UIManager.instance;
        grid = new ANode[gridSizeX, gridSizeY];
        u.UIgrids = new UnityEngine.UI.Image[gridSizeX, gridSizeY];
        
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
                u.UIgrids[x,y] = Instantiate(u.baseNodeImg,u.gridCanvas);
                u.UIgrids[x,y].rectTransform.anchoredPosition = new Vector2(worldPoint.x,worldPoint.z);
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
    public ANode GetNodeWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
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
        }
    }
}
