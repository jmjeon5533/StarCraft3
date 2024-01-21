using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ISubscriber
{
    public static UIManager instance { get; private set; }
    [SerializeField] GameObject unitInfo;
    [SerializeField] Transform selectBtnParent;
    [SerializeField] Sprite defaultSprite;
    Button[] selectBtns;
    Image[] selectImages = new Image[15];
    // [SerializeField] Button normalbuildBtn;
    // [SerializeField] Button advancebuildBtn;
    [HideInInspector] public Image[,] UIgrids;
    public Image baseNodeImg;
    public InfoUI baseUnitInfo;
    public Transform gridCanvas;
    public Transform infoCanvas;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        selectBtns = selectBtnParent.GetComponentsInChildren<Button>();
        for (int i = 0; i < 15; i++)
        {
            selectImages[i] = selectBtns[i].GetComponent<Image>();
        }
        ObserverManager.instance.AddSubscriber(this);
        ResetUI();

        // normalbuildBtn.onClick.AddListener(()=>{
        //     i.InitMode(i.keyInfo.BuildMode);
        // });
        // advancebuildBtn.onClick.AddListener(()=>{

        // });
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
        for (int x = 0; x < UIgrids.GetLength(0); x++)
        {
            for (int y = 0; y < UIgrids.GetLength(1); y++)
            {
                UIgrids[x, y].enabled = i.keyInfo.curState != i.keyInfo.MoveMode;
                bool isWalk = b.grid.grid[x, y].isBuildAble;
                UIgrids[x, y].color = isWalk ? new Color(0.7f, 0.7f, 0.7f, 0.5f) : new Color(1, 0, 0, 0.5f);
            }
        }
    }
    public void CreateUnitInfo(Unit unit)
    {
        var infoUI = Instantiate(baseUnitInfo,infoCanvas);
        infoUI.target = unit;
    }
    public void ResetUI()
    {
        for (int i = 0; i < selectBtns.Length; i++)
        {
            selectBtns[i].onClick.RemoveAllListeners();
            selectImages[i].sprite = defaultSprite;
        }
    }
    public void UnitUI(List<ButtonConstructor> skillList)
    {
        ResetUI();
        foreach (var skill in skillList)
        {
            var index = skill.btnXY.y + (skill.btnXY.x * 5);
            var btn = selectBtns[index];
            var img = selectImages[index];
            btn.onClick.AddListener(skill.Action);
            string path = $"Image/ButtonIcons/{skill.iconKey}";
            img.sprite = Resources.Load<Sprite>(path);
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
