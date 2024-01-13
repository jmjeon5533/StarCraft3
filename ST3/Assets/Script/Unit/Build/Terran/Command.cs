using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : Terran
{
    [SerializeField] GameObject miner;
    [SerializeField] private float trainDelay;

    private bool isTraining = false;
    private Queue<IEnumerator> trainMiner = new Queue<IEnumerator>();

    private void Awake()
    {
        constructors[0] = 
            new TrainUnit(new ButtonInfo
            {
                iconPath = "Image/ButtonIcons/Miner/MinerIcon",
                state = ButtonPanelState.BUILDINGS
            },this);
    }

    public override void Move(RaycastHit hit)
    {
        if(IsGround) return;
        targetIndex = 0;
        NavManager.RequestPath(transform.position, hit.point,OnPathFound);
    }

    public override void Skill()
    {
        if (!isTraining) StartCoroutine(Train());
        else trainMiner.Enqueue(Train());
    }

    private IEnumerator Train()
    {
        isTraining = true;
        yield return YieldReturnVariables.WaitForSeconds(trainDelay);
        //Debug.Log("Succeed Train Miner");
        ANode node = BuildManager.instance.grid.GetNodeWorldPoint(transform.position + (transform.localScale/10));
       
       
        Vector3 pos = transform.position + node.worldPos;
        Instantiate(miner, pos, Quaternion.identity);
        if (trainMiner.Count > 0)
        {
            StartCoroutine(trainMiner.Dequeue());
            yield break;
        }
        isTraining = false;
    }
}
