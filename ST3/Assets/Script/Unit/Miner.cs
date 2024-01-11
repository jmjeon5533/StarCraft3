using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Unit
{
    Mineral targetMineral;
    bool haveMineral;
    float curMiningTime;
    // List<ButtonUIInfo> mainBuilds = new List<ButtonUIInfo>();
    private void Awake()
    {
        // skills = new MinerSkill1();
    }
    public override void Move(RaycastHit hit)
    {
        targetMineral = null;
        switch (hit.collider.tag)
        {
            case "Ground": base.Move(hit); break;
            case "Mineral":
                {
                    targetMineral = hit.collider.GetComponent<Mineral>();
                    base.Move(hit);
                    break;
                }
        }
    }
    private void Update()
    {
        Mining();
    }
    private void Mining()
    {
        if(targetMineral == null){ curMiningTime = 0; return; }
        if(Vector3.Distance(targetMineral.transform.position,transform.position) <= 5)
        {
            curMiningTime += Time.deltaTime;
            if(curMiningTime >= 3)
            {
                haveMineral = targetMineral.GetMineral(5);

            }
        }
    }
}
// public class MinerSkill : UnitSkill
// {
//     public MinerSkill()
//     {
//         action = new NormalBuild();
//     }
// }
// public class NormalBuild : ButtonUIInfo
// {
//     List<ButtonUIInfo> normalBuilds = new List<ButtonUIInfo>();

//     public override void Action()
//     {
//         UIManager.instance.UnitUI(normalBuilds);
//     }
// }
// public class Advancebuild : ButtonUIInfo
// {
//     List<ButtonUIInfo> advanceBuilds = new List<ButtonUIInfo>();

//     public override void Action()
//     {
//         UIManager.instance.UnitUI(advanceBuilds);
//     }
// }




// public class MinerSkill1 : SkillInfo
// {
//     MinerSkill2 normalBuildInfo = new MinerSkill2();
//     public override void skill1()
//     {
//         UIManager.instance.UnitUI(normalBuildInfo.skillList);
//     }
//     public override void skill2()
//     {
//         UIManager.instance.UnitUI(normalBuildInfo.)
//     }
//     public override void skill3()
//     {

//     }
//     public override void skill4()
//     {

//     }
//     public override void skill5()
//     {

//     }
//     public override void skill6()
//     {

//     }
//     public override void skill7()
//     {

//     }
//     public override void skill8()
//     {

//     }
//     public override void skill9()
//     {

//     }
// }
// public class MinerSkill2 : SkillInfo
// {
//     public override void skill1()
//     {
//         var i = IngameManager.instance;
//         i.InitMode(i.keyInfo.BuildMode);

//         var command = BuildManager.instance.GetBuildDic("Command");

//         BuildManager.instance.selectBuildInfo = command;

//         BuildManager.instance.GhostInit(true, command.BuildScale);
//     }
//     public override void skill2()
//     {

//     }
//     public override void skill3()
//     {

//     }
//     public override void skill4()
//     {

//     }
//     public override void skill5()
//     {

//     }
//     public override void skill6()
//     {

//     }
//     public override void skill7()
//     {

//     }
//     public override void skill8()
//     {

//     }
//     public override void skill9()
//     {

//     }
// }
