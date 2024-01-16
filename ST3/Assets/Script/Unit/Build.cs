using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : Unit
{
    public Vector2Int BuildScale;
    public float buildTime;
    float curBuildTime;
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
