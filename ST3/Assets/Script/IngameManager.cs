using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    [SerializeField] Transform camPivot;
    [SerializeField] Camera cam;
    [Range(0,1)]
    [SerializeField] float scrollValue;
    void Start()
    {
        scrollValue = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        CamInput();
    }
    void CamInput()
    {
        scrollValue -= Input.GetAxis("Mouse ScrollWheel");
        scrollValue = Mathf.Clamp(scrollValue, 0, 1);
        cam.transform.localPosition = new Vector3(0,0,Mathf.Lerp(2,-3,scrollValue));

        
    }
}
