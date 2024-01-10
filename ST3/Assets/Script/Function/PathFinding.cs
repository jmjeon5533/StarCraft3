using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    NavManager navManager;
    AGrid grid;


    private void Awake()
    {
        navManager = GetComponent<NavManager>();
        grid = GetComponent<AGrid>();
    }

    public void StartFindPath(Vector3 startpos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startpos, targetPos));
    }
    IEnumerator FindPath(Vector3 startpos, Vector3 targetPos)
    {
        Vector3[] waypoint = new Vector3[0];
        bool pathSuccess = false;

        ANode startNode = grid.GetNodeWorldPoint(startpos);
        ANode targetNode = grid.GetNodeWorldPoint(targetPos);

        if (startNode.isWalkAble && targetNode.isWalkAble)
        {

            List<ANode> openList = new List<ANode>();
            HashSet<ANode> closedList = new HashSet<ANode>();
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                ANode currentNode = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < currentNode.fCost
                    || openList[i].fCost == currentNode.fCost
                    && openList[i].hCost < currentNode.hCost)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode == targetNode)
                {
                    // RetrancePath(startNode, targetNode);
                    pathSuccess = true;

                    break;
                }

                foreach (ANode n in grid.GetNeighbours(currentNode))
                {
                    if (!n.isWalkAble || closedList.Contains(n)) continue;

                    int newCurrentToNeighbourCost = currentNode.gCost + GetDistanceCost(currentNode, n);
                    if (newCurrentToNeighbourCost < n.gCost || !openList.Contains(n))
                    {
                        n.gCost = newCurrentToNeighbourCost;
                        n.hCost = GetDistanceCost(n, targetNode);
                        n.parentNode = currentNode;

                        if (!openList.Contains(n))
                        {
                            openList.Add(n);
                        }
                    }
                }
            }
        }
        yield return null;
        if(pathSuccess)
        {
            waypoint = RetrancePath(startNode, targetNode);
        }

        navManager.FinishedProcessingPath(waypoint, pathSuccess);
    }
    Vector3[] RetrancePath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }
    Vector3[] SimplifyPath(List<ANode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew 
            = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    int GetDistanceCost(ANode nodeA, ANode nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
