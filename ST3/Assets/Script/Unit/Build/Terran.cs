using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terran : Build
{
    private bool isground;
    public bool IsGround
    {
        get { return isground; }
        set { isground = value; }
    }
    protected override void Start()
    {
        base.Start();
        IsGround = true;
    }
}
