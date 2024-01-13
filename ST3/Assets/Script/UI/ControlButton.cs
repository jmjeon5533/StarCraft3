using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButton : MonoBehaviour
{
    ButtonConstructor constructor;
    Button button;
    
    public void SetConstruct(ButtonConstructor _constructor)
    {
        constructor = _constructor;
        constructor.Init(button);
        if(constructor.icon != null)
            button.image.sprite = constructor.icon;
    }
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out button);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
