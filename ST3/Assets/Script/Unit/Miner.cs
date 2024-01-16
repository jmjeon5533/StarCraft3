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
    public BuildRequest(Build build,Vector3 pos, Vector2Int cGridPos)
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
    BuildRequest curRequest;

    // List<ButtonUIInfo> mainBuilds = new List<ButtonUIInfo>();
    private void Awake()
    {
        // skills = new MinerSkill1();
    }
    public override void Move(Vector3 pos, RaycastHit hit = default)
    {
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
        var scanRange = curRequest.build.BuildScale.x/2;
        if (Vector3.Distance(curRequest.pos, transform.position) <= scanRange)
        {
            var b = BuildManager.instance;
            var obj = Instantiate(curRequest.build, curRequest.pos, Quaternion.identity);

            for (int i = 0; i < obj.BuildScale.x; i++)
            {
                for (int j = 0; j < obj.BuildScale.y; j++)
                {
                    var g = b.grid.grid[curRequest.cGridPos.x + i, curRequest.cGridPos.y + j];
                    g.isWalkAble = false;
                }
            }
            b.curBuilding.Add(obj);
            isRequest = false;
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
#endregion
public class AdvanceBuild : ButtonConstructor
{

    public override void Action()
    {
        Debug.Log("Advance");
    }
    public AdvanceBuild()
    {
        keyCode = KeyCode.V;
        btnXY = new Vector2Int(0, 4);
        iconKey = "Miner/AdvanceBuild";
    }
}
