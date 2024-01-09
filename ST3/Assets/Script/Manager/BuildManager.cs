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
    [SerializeField] buildInfo[] buildList;
    [SerializeField] AGrid grid;

    List<Build> curBuilding = new List<Build>();
    public void CreateBuild(string buildKey, Vector3 targetPos)
    {
        var UseGrid = grid.GetNodeBuildWorldPoint(targetPos);
        Build build;
        buildDic[buildKey].TryGetComponent(out build);
        Vector2Int scale = build.buildingSize;
       

        Vector2Int cGridPos = new Vector2Int(UseGrid.gridX - Mathf.RoundToInt(scale.x / 2)
        ,UseGrid.gridY - Mathf.RoundToInt(scale.y / 2));

        if (!CheckBuildNodes(cGridPos, scale))
        {
            Debug.Log("Cannot Build this area");
            return;
        }
        Instantiate(buildDic[buildKey], UseGrid.worldPos, Quaternion.identity);

        print($"{scale}, {UseGrid.gridX}:{UseGrid.gridY}, {cGridPos}");
        
        for (int i = 0; i < scale.x; i++)
        {
            for (int j = 0; j < scale.y; j++)
            {
                var g = grid.buildGrid[cGridPos.x + i, cGridPos.y + j];
                print($"Grid = {g.gridX},{g.gridY}");
                g.isWalkAble = g.SerchWalkAble(g.worldPos,0.1f,grid.unwalkAbleMask);
            }
        }
        UseGrid.isWalkAble = false;
        curBuilding.Add(buildDic[buildKey]);
    }

    public bool CheckBuildNodes(Vector2Int centerPos, Vector2Int scale)
    {
        for (int i = 0; i < scale.x; i++)
        {
            for (int j = 0; j < scale.y; j++)
            {
                var g = grid.buildGrid[centerPos.x + i, centerPos.y + j];
                if(!g.isWalkAble) return false;
            }
        }
        return true;
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
}
