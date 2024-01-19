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
    protected virtual void Start()
    {
        IsGround = true;
    }
}
