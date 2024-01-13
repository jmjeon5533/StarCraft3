using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonPanelState
{
    NONE = 0,
    BUILDINGS = 1,
    UNITCONTROL = 2,
    EXPLAIN = 3
}

public struct ButtonInfo
{
    public string iconPath;
    public ButtonPanelState state;
}

public class ButtonConstructor
{
    public KeyCode keycode;
    public ButtonPanelState state;
    public Sprite icon;
    public string iconKey;

    public ButtonConstructor(ButtonInfo info)
    {
        ButtonInfo buttonInfo = info;
        if(info.iconPath != "")
        {
            icon = Resources.Load<Sprite>(buttonInfo.iconPath);
            Debug.Log(info.iconPath); 
            Debug.Log(icon);
        } else
        {
            icon = Resources.Load<Sprite>("Image/ButtonIcons/Empty");
            Debug.Log(info.iconPath);
            Debug.Log(icon);
        }
        state = buttonInfo.state;
    }

    public virtual void Init(Button myButton)
    {
        if(myButton != null)
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(Action);
        }
        
    }

    public virtual void Action()
    {
            
    }
}
