using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral : MonoBehaviour
{
    public int saveMineral = 10000;
    public bool GetMineral(int getvalue)
    {
        saveMineral -= getvalue;
        return true;
    }
}
