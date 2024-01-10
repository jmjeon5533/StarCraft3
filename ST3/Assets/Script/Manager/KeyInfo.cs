using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IClick
{
    public void Init();
    public void LeftClick();
    public void RightClick();
}
public class MoveMode : IClick
{
    public void Init()
    {

    }
    public void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                switch(hit.collider.gameObject.tag)
                {
                    case "Ground":
                    case "Wall":
                    case "Untagged":
                    {
                        IngameManager.instance.curUnit.Clear();
                        break;
                    }
                    case "Unit":
                    {
                        IngameManager.instance.curUnit.Add(hit.collider.GetComponent<Unit>());
                        break;
                    }
                }
            }
    }
    public void RightClick()
    {
        
        if (IngameManager.instance.curUnit == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider)
                {
                    foreach(var unit in IngameManager.instance.curUnit)
                    {
                        unit.Move(hit);
                    }
                    // switch (hit.collider.tag)
                    // {

                    //     case "Ground":
                    //         foreach (var unit in IngameManager.instance.curUnit) unit.Move(hit.point);
                    //         break;
                    //     case "Mineral":
                    //         var pos = new Vector3(hit.point.x, 0, hit.point.z);
                    //         foreach (var unit in IngameManager.instance.curUnit) unit.Move(pos);
                    //         break;
                    // }
                }
            }
    }   
}
public class BuildMode : IClick
{
    public void Init()
    {
        
    }
    public void LeftClick()
    {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                switch(hit.collider.gameObject.tag)
                {
                    case "Ground":
                    {
                        BuildManager.instance.CreateBuild("Command",hit.point);
                        break;
                    }
                    default : return;
                }
                ObserverManager.instance.NotifyToSubscriber();
            }
    }
    public void RightClick()
    {

    }
}
public class KeyInfo
{
    public readonly IClick MoveMode;
    public readonly IClick normalBuildMode;
    public KeyInfo()
    {
        MoveMode = new MoveMode();
        normalBuildMode = new BuildMode();

        curState = MoveMode;
    }
    public IClick curState;

    public void LeftClick() => curState.LeftClick();
    public void RightClick() => curState.RightClick();
}
