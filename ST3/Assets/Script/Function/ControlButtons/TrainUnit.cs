using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainUnit : ButtonConstructor
{
    public Build building;

    public TrainUnit(ButtonInfo info, Build _building) : base(info)
    {
        building = _building;
    }

    public override void Init(Button myButton)
    {
        base.Init(myButton);
    }

    public override void Action()
    {
        building.Skill(); 
    } 

    
}
