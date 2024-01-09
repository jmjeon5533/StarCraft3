using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Unit
{
    Mineral targetMineral;
    bool haveMineral;
    float curMiningTime;
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
