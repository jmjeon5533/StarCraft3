using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode
{
    public bool isWalkAble;
    public bool isBuildAble;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public ANode parentNode;

    public ANode(bool nWalkable, Vector3 nWorldPos, int nGridX, int nGridY)
    {
        isWalkAble = nWalkable;
        isBuildAble = nWalkable;
        worldPos = nWorldPos;
        gridX = nGridX;
        gridY = nGridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }
}
