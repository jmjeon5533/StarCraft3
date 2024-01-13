using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YieldReturnVariables
{
    private static readonly Dictionary<float, WaitForSeconds> _waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float time)
    {
        WaitForSeconds waitForSeconds;
        if (_waitForSeconds.TryGetValue(time, out waitForSeconds) == false)
        {
            waitForSeconds = new WaitForSeconds(time);
            _waitForSeconds.Add(time, waitForSeconds);
        }
        return waitForSeconds;
    }

}
