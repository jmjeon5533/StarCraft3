using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Supply_Depot : Terran
{
    [HideInInspector]
    public Animator anim;
    public bool IsLand = false;
    public override void Move(Vector3 pos, RaycastHit hit = default)
    {
        if(IsGround) return;
        targetIndex = 0;
        NavManager.RequestPath(transform.position, hit.point,OnPathFound);
    }
    public override List<ButtonConstructor> GetButtonInfo()
    {
        List<ButtonConstructor> list = new List<ButtonConstructor>();

        list.Add(new Supply_Updown(this));

        return list;
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
public class Supply_Updown : ButtonConstructor
{
    Supply_Depot supply;
    string[] img = new string[2];
    public Supply_Updown(Supply_Depot supply_)
    {
        supply = supply_;
        keyCode = KeyCode.R;
        btnXY = new Vector2Int(0,4);
        img[0] = "Supply_Depot/Supply_Down";
        img[1] = "Supply_Depot/Supply_Up";
        iconKey = LandSpriteKey();
    }
    public override void Action()
    {
        if(supply.curBuildTime < supply.buildTime) return;
        supply.IsLand = !supply.IsLand;
        supply.anim.SetBool("IsLand",supply.IsLand);
        BuildManager.instance.WalkAbleOnOff(supply,supply.IsLand);
        iconKey = LandSpriteKey();
        UIManager.instance.UnitUI(supply.GetButtonInfo());
    }
    string LandSpriteKey()
    {
        return supply.IsLand ? img[1] : img[0];
    }
}
