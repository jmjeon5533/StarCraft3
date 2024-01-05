using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    float speed = 10;
    Vector3[] path;
    int targetIndex;
    Coroutine curPath;

    void Start()
    {
            
    }
    private void Update()
    {
        
    }
    public void Move(Vector3 TargetPos)
    {
        targetIndex = 0;
        NavManager.RequestPath(transform.position, TargetPos,OnPathFound);
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
            print(Vector3.Distance(transform.position, currentWaypoint));
            if(Vector3.Distance(transform.position, currentWaypoint) <= 0.01f)
            {
                targetIndex++;
                if(targetIndex == path.Length)
                {
                    print("End");
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
            yield return null;
        }
    }
}
