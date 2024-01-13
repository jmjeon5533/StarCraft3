using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public interface IClick
{
    public void Init();
    public void LeftClick();
    public void RightClick();
    public void Loop();
}
public class MoveMode : IClick
{
    public void Init()
    {
        BuildManager.instance.selectBuildInfo = null;
        BuildManager.instance.GhostInit(false);
    }
    public void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Ground":
                    case "Wall":
                    case "Untagged":
                        {
                            IngameManager.instance.ResetSelectUnit();
                            break;
                        }
                    case "Unit":
                        {
                            IngameManager.instance.AddSelectUnit(hit.collider.GetComponent<Unit>());
                            break;
                        }
                }
            }
    }
    public void RightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (IngameManager.instance.curUnit == null) return;
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                if (hit.collider)
                {
                    foreach (var unit in IngameManager.instance.curUnit)
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
    public void Loop()
    {

    }
}
public class BuildMode : IClick
{
    public void Init()
    {
        var command = BuildManager.instance.GetBuildDic("Command");

        BuildManager.instance.selectBuildInfo = command;

        BuildManager.instance.GhostInit(true, command.BuildScale);
    }
    public void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Ground":
                        {
                            BuildManager.instance.CreateBuild("Command", hit.point);
                            break;
                        }
                    default: return;
                }
                ObserverManager.instance.NotifyToSubscriber();
            }
    }
    public void RightClick()
    {

    }
    public void Loop()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    BuildManager.instance.GhostGridMove(hit.point);
                }
                else
                {
                    BuildManager.instance.GhostWorldMove(hit.point);
                }
            }

    }
}
public class KeyInfo
{
    public readonly IClick MoveMode;
    public readonly IClick BuildMode;
    public KeyInfo()
    {
        MoveMode = new MoveMode();
        BuildMode = new BuildMode();

        curState = MoveMode;
    }
    public IClick curState;

    public void Init() => curState.Init();
    public void LeftClick() => curState.LeftClick();
    public void RightClick() => curState.RightClick();
    public void Loop() => curState.Loop();
}
