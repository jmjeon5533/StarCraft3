using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    float speed = 10;
    Vector3[] path;
    protected int targetIndex;
    Coroutine curPath;

    protected ButtonConstructor[] constructors = new ButtonConstructor[9];

    public virtual void Move(RaycastHit hit)
    {
        targetIndex = 0;
        NavManager.RequestPath(transform.position, hit.point,OnPathFound);
    }

    public virtual void Skill()
    {

    }

    public virtual ButtonConstructor[] ConstructButton()
    {
        return constructors;
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
}
