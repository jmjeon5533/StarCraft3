using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ISubscriber
{
    public static UIManager instance {get; private set;}
    [SerializeField] GameObject unitInfo;
    [SerializeField] Button normalbuildBtn;
    [SerializeField] Button advancebuildBtn;
    [HideInInspector] public Image[,] UIgrids;
    public Image baseNodeImg;
    public Transform gridCanvas;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        var i = IngameManager.instance;
        ObserverManager.instance.AddSubscriber(this);
        normalbuildBtn.onClick.AddListener(()=>{
            i.InitMode(i.keyInfo.BuildMode);
        });
        advancebuildBtn.onClick.AddListener(()=>{
            
        });
        TotalInit();
    }
    private void Update()
    {
        var i = IngameManager.instance;
        unitInfo.SetActive(i.IsUnitSelect());
    }
    public void TotalInit()
    {
        var i = IngameManager.instance;
        var b = BuildManager.instance;
        for(int x = 0; x < UIgrids.GetLength(0); x++)
        {
            for(int y = 0; y < UIgrids.GetLength(1); y++)
            {
                UIgrids[x,y].enabled = i.keyInfo.curState != i.keyInfo.MoveMode;
                bool isWalk = b.grid.grid[x,y].isWalkAble;
                UIgrids[x,y].color = isWalk ? new Color(0,1,1,0.5f) : new Color(1,0,0,0.5f);
            }
        }
    }
    // public void UnitUI(List<ButtonConstructor> skillList)
    // {
    //     var g = IngameManager.instance;
    //     for(int i = 0; i < Btn.Length; i++)
    //     {
    //         var num = i;
    //         Btn[num].onClick.RemoveAllListeners();
    //         Btn[num].onClick.AddListener(() => {
    //             skillList[num].Action();
    //         });
    //     }
    // }
}
