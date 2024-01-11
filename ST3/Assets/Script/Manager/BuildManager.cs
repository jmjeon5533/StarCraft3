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

    List<Build> curBuilding = new List<Build>();
    public void CreateBuild(string buildKey, Vector3 targetPos)
    {
        var UseGrid = grid.GetNodeWorldPoint(targetPos);
        var BuildScale = buildDic[buildKey].BuildScale;

        Vector2Int cGridPos = new Vector2Int(UseGrid.gridX - Mathf.RoundToInt(BuildScale.x / 2)
        ,UseGrid.gridY - Mathf.RoundToInt(BuildScale.y / 2));

        if(!CheckWalkable(cGridPos,BuildScale))
        {
            print("Don't Build");
            return;
        }
        var obj = Instantiate(buildDic[buildKey], UseGrid.worldPos, Quaternion.identity);

        print($"{obj.BuildScale}, {UseGrid.gridX}:{UseGrid.gridY}, {cGridPos}");
        
        for (int i = 0; i < obj.BuildScale.x; i++)
        {
            for (int j = 0; j < obj.BuildScale.y; j++)
            {
                var g = grid.grid[cGridPos.x + i, cGridPos.y + j];
                g.isWalkAble = false;
            }
        }
        UseGrid.isWalkAble = false;
        curBuilding.Add(buildDic[buildKey]);
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
                if(!g.isWalkAble) return false;
            }
        }
        return true;
    }
    public void GhostInit(bool isActive, Vector2Int scale = default)
    {
        selectGhost.SetActive(isActive);
        selectGhost.transform.localScale = new Vector3(scale.x,3,scale.y) * 0.2f;
    }
    public void GhostGridMove(Vector3 pos)
    {
        var gridPos = grid.GetNodeWorldPoint(pos);
        selectGhost.transform.position = gridPos.worldPos;

        Vector2Int cGridPos = 
        new Vector2Int(gridPos.gridX - Mathf.RoundToInt(selectBuildInfo.BuildScale.x / 2)
        ,gridPos.gridY - Mathf.RoundToInt(selectBuildInfo.BuildScale.y / 2));

        Color WalkableColor = new Color(0.25f,1,1,0.5f);
        Color WalkUnableColor = new Color(1,0,0,0.5f);

        ghostMaterial.color = CheckWalkable(cGridPos,selectBuildInfo.BuildScale) 
        ? WalkableColor : WalkUnableColor;
    }
    public void GhostWorldMove(Vector3 pos)
    {
        selectGhost.transform.position = pos;
        Color WalkUnableColor = new Color(1,0,0,0.5f);

        ghostMaterial.color = WalkUnableColor;
    }
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
}
