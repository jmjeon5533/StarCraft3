using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;

struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public UnityAction<Vector3[], bool> callback;

    public PathRequest(Vector3 nStart, Vector3 nEnd, UnityAction<Vector3[], bool> nCallback)
    {
        pathStart = nStart;
        pathEnd = nEnd;
        callback = nCallback;
    }
}
public class NavManager : MonoBehaviour
{
    Queue<PathRequest> RequestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    static NavManager instance;
    PathFinding pathFinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, UnityAction<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.RequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    void TryProcessNext()
    {
        if(!isProcessingPath && RequestQueue.Count > 0)
        {
            currentRequest = RequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentRequest.pathStart,currentRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentRequest.callback(path,success);
        isProcessingPath = false;
        TryProcessNext();
    }
}
