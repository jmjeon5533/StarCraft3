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
    [SerializeField] Grid buildGrid;

    [SerializeField] Camera cam;

    private GameObject previewBuild;

    public void ShowBuild(string buildKey)
    {
        Vector3Int gridPosition = Vector3Int.zero;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,100))
        {
            gridPosition = buildGrid.WorldToCell(hit.point);
        }

        if(previewBuild == null)
            previewBuild = Instantiate(buildDic[buildKey], gridPosition, Quaternion.identity).gameObject;
        MeshRenderer meshRenderer;
        previewBuild.TryGetComponent(out meshRenderer);
        Color color = meshRenderer.material.GetColor("_Color");
        meshRenderer.material.SetColor("_Color", new Color(color.r,color.g,color.b,0.4f));
        previewBuild.transform.position = buildGrid.CellToWorld(gridPosition);
    }

    public bool CheckBuildMode()
    {
        var g = IngameManager.instance;
        if (g.keyInfo.curState.GetType() == typeof(BuildMode))
        {
            return true;
        } else
        {
            return false;
        }
    }

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
        buildGrid.cellSize = new Vector3(grid.BuildNodeDiameter, 0, grid.BuildNodeDiameter);
    }

    private void Update()
    {
        if(CheckBuildMode())
        {
            Debug.Log("Test");
            ShowBuild("Command");
        }
    }
}
