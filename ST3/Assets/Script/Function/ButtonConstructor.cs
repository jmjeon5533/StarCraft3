using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonConstructor
{
    public KeyCode keyCode;
    public string iconKey;
    public Vector2Int btnXY;
    public abstract void Action();
}
