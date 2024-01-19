using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class buildInfo
{
    public string key;
    public Build value;
}
public class BuildManager : MonoBehaviour
{
    public static BuildManager instance { get; private set; }
    Dictionary<string, Build> buildDic = new Dictionary<string, Build>();
    [SerializeField] public Build selectBuildInfo;
    [SerializeField] buildInfo[] buildList;
    [SerializeField] GameObject selectGhost;
    [SerializeField] Material ghostMaterial;
    public AGrid grid;

    public List<Build> curBuilding = new List<Build>();
    public void CreateBuild(Vector3 targetPos)
    {
        var g = IngameManager.instance;
        var UseGrid = grid.GetNodeWorldPoint(targetPos);
        var BuildScale = selectBuildInfo.BuildScale;

        Vector2Int cGridPos = new Vector2Int(UseGrid.gridX - Mathf.RoundToInt(BuildScale.x / 2)
        , UseGrid.gridY - Mathf.RoundToInt(BuildScale.y / 2));

        if (!CheckBuildable(cGridPos, BuildScale))
        {
            print("Don't Build");
            return;
        }
        var miner = g.curUnit[0].GetComponent<Miner>();
        miner.buildRequest.Enqueue(new BuildRequest(selectBuildInfo, UseGrid.worldPos, cGridPos));
        miner.Move(grid.grid[cGridPos.x - 1, cGridPos.y - 1].worldPos);

        g.InitMode(g.keyInfo.MoveMode);
        UIManager.instance.ResetUI();
        g.curUnit.Clear();

    }
    public void WalkAbleOnOff(Build obj, bool isActive)
    {
        for (int i = 0; i < obj.BuildScale.x; i++)
        {
            for (int j = 0; j < obj.BuildScale.y; j++)
            {
                var g = grid.grid[obj.cGridPos.x + i, obj.cGridPos.y + j];
                g.isWalkAble = isActive;
            }
        }
    }
    public void BuildAbleOnOff(Build obj, bool isActive)
    {
        for (int i = 0; i < obj.BuildScale.x; i++)
        {
            for (int j = 0; j < obj.BuildScale.y; j++)
            {
                var g = grid.grid[obj.cGridPos.x + i, obj.cGridPos.y + j];
                g.isBuildAble = isActive;
            }
        }
    }
    public Build GetBuildDic(string key)
    {
        return buildDic[key];
    }
    public bool CheckWalkable(Vector2Int leftDownPos, Vector2Int scale)
    {
        for (int i = 0; i < scale.x; i++)
        {
            for (int j = 0; j < scale.y; j++)
            {
                var g = grid.grid[leftDownPos.x + i, leftDownPos.y + j];
                if (!g.isWalkAble) return false;
            }
        }
        return true;
    }
    public bool CheckBuildable(Vector2Int leftDownPos, Vector2Int scale)
    {
        for (int i = 0; i < scale.x; i++)
        {
            for (int j = 0; j < scale.y; j++)
            {
                var g = grid.grid[leftDownPos.x + i, leftDownPos.y + j];
                if (!g.isBuildAble) return false;
            }
        }
        return true;
    }
    #region GhostSet
    public void GhostInit(bool isActive, Vector2Int scale = default)
    {
        selectGhost.SetActive(isActive);
        selectGhost.transform.localScale = new Vector3(scale.x, 3, scale.y) * 0.2f;
    }
    public void GhostGridMove(Vector3 pos)
    {
        var gridPos = grid.GetNodeWorldPoint(pos);
        selectGhost.transform.position = gridPos.worldPos;

        Vector2Int cGridPos =
        new Vector2Int(gridPos.gridX - Mathf.RoundToInt(selectBuildInfo.BuildScale.x / 2)
        , gridPos.gridY - Mathf.RoundToInt(selectBuildInfo.BuildScale.y / 2));

        Color WalkableColor = new Color(0.25f, 1, 1, 0.5f);
        Color WalkUnableColor = new Color(1, 0, 0, 0.5f);

        ghostMaterial.color = CheckBuildable(cGridPos, selectBuildInfo.BuildScale)
        ? WalkableColor : WalkUnableColor;
    }
    public void GhostWorldMove(Vector3 pos)
    {
        selectGhost.transform.position = pos;
        Color WalkUnableColor = new Color(1, 0, 0, 0.5f);

        ghostMaterial.color = WalkUnableColor;
    }
    #endregion
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        foreach (var b in buildList)
        {
            buildDic.Add(b.key, b.value);
        }
    }
    private void Update()
    {

    }
    public void SetBuildObj(string BuildName)
    {
        var g = IngameManager.instance;
        var obj = GetBuildDic(BuildName);

        selectBuildInfo = obj;

        GhostInit(true, obj.BuildScale);
        g.InitMode(g.keyInfo.BuildMode);
    }
}
