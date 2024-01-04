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
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, Mathf.Infinity,LayerMask.GetMask("Ground")))
            {
                if(hit.collider)
                {
                    targetIndex = 0;
                    NavManager.RequestPath(transform.position, hit.point,OnPathFound);
                }
            }
        }
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
