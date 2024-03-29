using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager instance { get; private set; }
    public KeyInfo keyInfo;
    [Header("카메라")]
    [SerializeField] Transform camPivot;
    [SerializeField] Camera cam;
    public float mouseTriggerRange;
    public float camSpeed;
    [Range(0, 1)]
    [SerializeField] float scrollValue;
    [Space(10)]
    [Header("유닛 관련")]
    public List<Unit> curUnit = new List<Unit>();
    public int curUnitIndex = 0;

    public bool IsUnitSelect()
    {
        return curUnit.Count > 0;
    }

    public void AddSelectUnit(Unit addUnit)
    {
        curUnit.Add(addUnit);
        UIManager.instance.LoadUnitButtons(addUnit);
    }

    public void ResetSelectUnit()
    {
        curUnit.Clear();
        UIManager.instance.LoadUnitButtons(null);
    }

    private void Awake()
    {
        instance = this;
        keyInfo = new KeyInfo();
    }
    void Start()
    {
        scrollValue = 0.5f;
        InitMode(keyInfo.MoveMode);
    }
    public void InitMode(IClick mode)
    {
        keyInfo.curState = mode;
        ObserverManager.instance.NotifyToSubscriber();
        keyInfo.Init();
    }

    // Update is called once per frame
    void Update()
    {
        CamInput();
        PlayerInput();
        keyInfo.Loop();
    }
    void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            keyInfo.LeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            keyInfo.RightClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InitMode(keyInfo.MoveMode);
        }
    }
    void CamInput()
    {
        scrollValue -= Input.GetAxis("Mouse ScrollWheel");
        scrollValue = Mathf.Clamp(scrollValue, 0, 1);
        cam.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(7, 0, scrollValue));
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
