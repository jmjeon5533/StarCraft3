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
            i.InitMode(i.keyInfo.normalBuildMode);
        });
        advancebuildBtn.onClick.AddListener(()=>{
            
        });
    }
    private void Update()
    {
        var i = IngameManager.instance;
        unitInfo.SetActive(i.IsUnitSelect());
    }
}
