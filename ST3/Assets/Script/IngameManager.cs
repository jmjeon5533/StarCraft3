using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager instance {get; private set;}
    [Header("카메라")]
    [SerializeField] Transform camPivot;
    [SerializeField] Camera cam;
    public float mouseTriggerRange;
    public float camSpeed;
    [Range(0, 1)]
    [SerializeField] float scrollValue;
    [Space(10)]
    [Header("유닛 관련")]
    [SerializeField] List<Unit> curUnit = new List<Unit>();
    public bool IsUnitSelect()
    {
        return curUnit.Count > 0;
    }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scrollValue = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        CamInput();
        MouseInput();
    }
    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                switch(hit.collider.gameObject.tag)
                {
                    case "Ground":
                    case "Wall":
                    case "Untagged":
                    {
                        curUnit.Clear();
                        break;
                    }
                    case "Unit":
                    {
                        curUnit.Add(hit.collider.GetComponent<Unit>());
                        break;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(curUnit == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                if (hit.collider)
                {
                    foreach(var unit in curUnit)
                    {
                        unit.Move(hit.point);
                    }
                }
            }
        }
    }
    void CamInput()
    {
        scrollValue -= Input.GetAxis("Mouse ScrollWheel");
        scrollValue = Mathf.Clamp(scrollValue, 0, 1);
        cam.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(2, -3, scrollValue));
        camPivot.transform.localPosition += CalCulateCamMovePos() * camSpeed * Time.deltaTime;
    }
    Vector3 CalCulateCamMovePos()
    {
        var triggerLeft = mouseTriggerRange;
        var triggerRight = Screen.width - mouseTriggerRange;
        var triggerTop = Screen.height - mouseTriggerRange;
        var triggerBottom = mouseTriggerRange;
        var m = Input.mousePosition;

        Vector3 returnPos = Vector3.zero;

        if (m.x <= triggerLeft) returnPos += Vector3.left;
        else if (m.x >= triggerRight) returnPos += Vector3.right;

        if (m.y <= triggerBottom) returnPos += Vector3.back;
        else if (m.y >= triggerTop) returnPos += Vector3.forward;

        return returnPos;
    }
}
