using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : Terran
{
    public Miner scv;
    public Vector3 summonPoint;
    public Vector3 rallyPoint;
    public Queue<SummonRequest> requests = new Queue<SummonRequest>();
    float curtime;
    bool isRequest = false;
    SummonRequest curRequest;
    public override void Move(Vector3 pos, RaycastHit hit = default)
    {
        if (IsGround)
        {
            rallyPoint = pos;
        }
        else return;
        targetIndex = 0;
        NavManager.RequestPath(transform.position, hit.point, OnPathFound);
    }
    protected override void Start()
    {
        base.Start();
        summonPoint = transform.position + new Vector3(-0.8f, 0, -0.8f);
        rallyPoint = summonPoint;
    }
    public override List<ButtonConstructor> GetButtonInfo()
    {
        List<ButtonConstructor> list = new List<ButtonConstructor>();

        list.Add(new Command_Build_SCV(this));

        return list;
    }
    private void Update()
    {
        if (requests.Count > 0 && !isRequest)
        {
            curRequest = requests.Dequeue();
            isRequest = true;
            curtime = 0;
        }
        if (isRequest)
        {
            curtime += Time.deltaTime;
            if (curtime > curRequest.summonTime)
            {
                var miner = Instantiate(scv, summonPoint, Quaternion.identity);
                miner.Move(rallyPoint);
                isRequest = false;
            }
        }
    }
}
public class Command_Build_SCV : ButtonConstructor
{
    Command command;
    public override void Action()
    {
        if (command.requests.Count < 5)
            command.requests.Enqueue(new SummonRequest(command.scv, command.rallyPoint, 5));
    }
    public Command_Build_SCV(Command command)
    {
        this.command = command;
        keyCode = KeyCode.S;
        iconKey = "Miner/SCV";
        btnXY = new Vector2Int(0, 4);
    }
}
