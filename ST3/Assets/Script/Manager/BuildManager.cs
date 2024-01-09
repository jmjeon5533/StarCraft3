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
        var UseGrid = grid.GetNodeFromWorldPoint(targetPos);
        var obj = Instantiate(buildDic[buildKey], UseGrid.worldPos, Quaternion.identity).transform;
        
        Vector2Int scale = new Vector2Int(Mathf.RoundToInt(obj.localScale.x * grid.nodeRadius * 15),
        Mathf.RoundToInt(obj.localScale.z * grid.nodeRadius * 15));


        Vector2Int cGridPos = new Vector2Int(UseGrid.gridX - Mathf.RoundToInt(scale.x)
        ,UseGrid.gridY - Mathf.RoundToInt(scale.y));

        print($"{scale}, {UseGrid.gridX}:{UseGrid.gridY}, {cGridPos}");
        
        for (int i = 0; i < scale.x; i++)
        {
            for (int j = 0; j < scale.y; j++)
            {
                var g = grid.grid[cGridPos.x + i, cGridPos.y + j];
                print($"Grid = {g.gridX},{g.gridY}");
                g.isWalkAble = g.SerchWalkAble(g.worldPos,grid.nodeRadius,grid.unwalkAbleMask);
            }
        }
        UseGrid.isWalkAble = false;
        curBuilding.Add(buildDic[buildKey]);
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
