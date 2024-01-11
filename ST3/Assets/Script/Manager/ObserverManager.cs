using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISignal
{
    public void AddSubscriber(ISubscriber ops);
    public void RemoveSubscriber(ISubscriber ops);
    public void NotifyToSubscriber();
}
public interface ISubscriber
{
    public void TotalInit();
}
public class ObserverManager : MonoBehaviour, ISignal
{
    public static ObserverManager instance {get; private set;}

    private void Awake()
    {
        instance = this;
    }
    List<ISubscriber> subscribers = new List<ISubscriber>();
    public void AddSubscriber(ISubscriber subs)
    {
        subscribers.Add(subs);
    }
    public void RemoveSubscriber(ISubscriber subs)
    {
        if(subscribers.IndexOf(subs) > 0) subscribers.Remove(subs);
    }
    public void NotifyToSubscriber()
    {
        foreach (var subs in subscribers) subs.TotalInit();
    }
}
