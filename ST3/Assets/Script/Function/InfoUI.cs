using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    public Unit target;
    RectTransform rect;
    public Image hp, doingTime;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        var pos = target.transform.position + (Vector3.up * 1.5f);
        rect.anchoredPosition = IngameManager.instance.cam.WorldToScreenPoint(pos);
        hp.fillAmount = target.hp / target.maxHP;
        doingTime.fillAmount = target.curtime / target.doingTime;
    }
}
