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
        
        Vector2Int cGridPos = new Vector2Int(UseGrid.gridX - Mathf.RoundToInt(obj.localScale.x * 2.5f)
        ,UseGrid.gridY - Mathf.RoundToInt(obj.localScale.z * 2.5f)); 
        for (int i = 0; i < Mathf.RoundToInt(obj.localScale.x * 5); i++)
        {
            for (int j = 0; j < Mathf.RoundToInt(obj.localScale.z * 5); j++)
            {
                grid.grid[cGridPos.x + i, cGridPos.y + j].isWalkAble = false;
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
