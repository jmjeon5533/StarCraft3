using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float hp,maxHP;
    public float curtime;
    public float doingTime;
    public float summonTime;
    float speed = 10;
    Vector3[] path;
    protected int targetIndex;
    Coroutine curPath;

    protected virtual void Start()
    {
        UIManager.instance.CreateUnitInfo(this);
    }

    public virtual void Move(Vector3 pos, RaycastHit hit = default)
    {
        targetIndex = 0;
        NavManager.RequestPath(transform.position, pos,OnPathFound);
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            if(curPath != null) 
            StopCoroutine(curPath);
            curPath = StartCoroutine(FollowPath());
        }
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while(true)
        {
            if(Vector3.Distance(transform.position, currentWaypoint) <= 0.01f)
            {
                targetIndex++;
                if(targetIndex == path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
            yield return null;
        }
    }
    public virtual List<ButtonConstructor> GetButtonInfo()
    {
        return null;
    }
}
