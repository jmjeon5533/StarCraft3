using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuildRequest
{
    public Vector3 pos;
    public Vector2Int cGridPos;
    public Build build;
    public BuildRequest(Build build, Vector3 pos, Vector2Int cGridPos)
    {
        this.build = build;
        this.pos = pos;
        this.cGridPos = cGridPos;
    }
}
public class Miner : Unit
{
    public Mineral targetMineral;
    bool haveMineral;
    float curMiningTime;

    public Queue<BuildRequest> buildRequest = new Queue<BuildRequest>();
    public bool isRequest;
    public bool isBuilding;
    BuildRequest curRequest;
    Build curBuildObj;

    // List<ButtonUIInfo> mainBuilds = new List<ButtonUIInfo>();
    private void Awake()
    {
        // skills = new MinerSkill1();
    }
    public override void Move(Vector3 pos, RaycastHit hit = default)
    {
        if (isRequest) return;
        targetMineral = null;
        if (hit.collider == null)
        {
            base.Move(pos);
        }
        else
        {
            switch (hit.collider.tag)
            {
                case "Ground": base.Move(hit.point); break;
                case "Mineral":
                    {
                        targetMineral = hit.collider.GetComponent<Mineral>();
                        base.Move(hit.point);
                        break;
                    }
            }
        }
        print("MoveMiner");
    }
    private void Update()
    {
        Mining();
        Building();
    }
    private void Mining()
    {
        if (targetMineral == null) { curMiningTime = 0; return; }
        if (Vector3.Distance(targetMineral.transform.position, transform.position) <= 5)
        {
            curMiningTime += Time.deltaTime;
            if (curMiningTime >= 3)
            {
                haveMineral = targetMineral.GetMineral(5);

            }
        }
    }
    public void Building()
    {
        if (buildRequest.Count > 0)
        {
            curRequest = buildRequest.Dequeue();
            isRequest = true;
        }
        if (!isRequest) return;

        var b = BuildManager.instance;
        var scanRange = curRequest.build.BuildScale.x / 2;
        if (Vector3.Distance(curRequest.pos, transform.position) <= scanRange)
        {
            if (!isBuilding)
            {
                isBuilding = true;
                curBuildObj = Instantiate(curRequest.build, curRequest.pos, Quaternion.identity);

                for (int i = 0; i < curBuildObj.BuildScale.x; i++)
                {
                    for (int j = 0; j < curBuildObj.BuildScale.y; j++)
                    {
                        var g = b.grid.grid[curRequest.cGridPos.x + i, curRequest.cGridPos.y + j];
                        g.isWalkAble = false;
                        g.isBuildAble = false;
                    }
                }
                curBuildObj.cGridPos = curRequest.cGridPos;
            }
            else
            {
                curBuildObj.curBuildTime += Time.deltaTime;
                var i = Mathf.Lerp(0, curBuildObj.maxHP, Mathf.InverseLerp(0, curBuildObj.buildTime, curBuildObj.curBuildTime));
                curBuildObj.hp = i;
                if (curBuildObj.curBuildTime >= curBuildObj.buildTime)
                {
                    b.curBuilding.Add(curBuildObj);
                    isBuilding = false;
                    isRequest = false;
                }
            }
        }
    }
    public override List<ButtonConstructor> GetButtonInfo()
    {
        List<ButtonConstructor> list = new List<ButtonConstructor>();

        list.Add(new NormalBuild());
        list.Add(new AdvanceBuild());

        return list;
    }
}

#region NormalBuild
public class NormalBuild : ButtonConstructor
{
    List<ButtonConstructor> list = new List<ButtonConstructor>();
    public override void Action()
    {
        UIManager.instance.UnitUI(list);
        Debug.Log("Normal");
    }
    public NormalBuild()
    {
        keyCode = KeyCode.B;
        btnXY = new Vector2Int(0, 3);
        iconKey = "Miner/NormalBuild";
        list.Add(new Build_Command());
        list.Add(new Build_Supply());
    }
}

public class Build_Command : ButtonConstructor
{
    public override void Action()
    {
        BuildManager.instance.SetBuildObj("Command");
    }
    public Build_Command()
    {
        keyCode = KeyCode.C;
        btnXY = new Vector2Int(0, 4);
        iconKey = "Miner/Command";
    }
}
public class Build_Supply : ButtonConstructor
{
    public override void Action()
    {
        BuildManager.instance.SetBuildObj("Supply_Depot");
    }
    public Build_Supply()
    {
        keyCode = KeyCode.S;
        btnXY = new Vector2Int(1, 3);
        iconKey = "Miner/Supply";
    }
}
#endregion

#region AdvanceBuild
public class AdvanceBuild : ButtonConstructor
{
    public override void Action()
    {
        Debug.Log("Advance");
    }
    public AdvanceBuild()
    {
        keyCode = KeyCode.V;
        btnXY = new Vector2Int(0, 3);
        iconKey = "Miner/AdvanceBuild";
    }
}
#endregion
