using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SummonRequest
{
    public Vector3 rallyPoint;
    public Unit unit;
    public float summonTime;
    public SummonRequest(Unit unit, Vector3 rallyPoint,float summonTime)
    {
        this.unit = unit;
        this.rallyPoint = rallyPoint;
        this.summonTime = summonTime;
    }
}
public class Build : Unit
{
    public Vector2Int BuildScale;
    public float buildTime;
    public float curBuildTime;
    public Vector2Int cGridPos;
    public override void Move(Vector3 pos, RaycastHit hit = default)
    {
        
    }
    public bool Building()
    {
        if(curBuildTime >= buildTime) return true;
        curBuildTime += Time.deltaTime;
        return false;   
    }
}
