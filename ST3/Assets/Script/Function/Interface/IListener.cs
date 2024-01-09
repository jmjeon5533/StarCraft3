using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event_Type
{
    TargetToMoveEvent,
    TargetToAttckEvent,
    TargetSelectEvent
}

public interface IListener
{
    public void OnNotify(EventType type, Component sender, object param = null);
}
