using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject unitInfo;
    [SerializeField] Button normalbuildBtn;
    [SerializeField] Button advancebuildBtn;
    private void Start()
    {
        var i = IngameManager.instance;
        normalbuildBtn.onClick.AddListener(()=>{
            i.keyInfo.curState = i.keyInfo.normalBuildMode;
        });
        advancebuildBtn.onClick.AddListener(()=>{
            i.keyInfo.curState = i.keyInfo.advanceBuildMode;
        });
    }
    private void Update()
    {
        var g = IngameManager.instance;
        unitInfo.SetActive(g.IsUnitSelect());
    }
}
