using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject unitInfo;
    private void Start()
    {
        
    }
    private void Update()
    {
        var g = IngameManager.instance;
        unitInfo.SetActive(g.IsUnitSelect());
    }
}
