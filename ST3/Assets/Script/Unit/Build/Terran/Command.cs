using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : Terran
{
    public override void Move(RaycastHit hit)
    {
        if(IsGround) return;
        targetIndex = 0;
        NavManager.RequestPath(transform.position, hit.point,OnPathFound);
    }
}
