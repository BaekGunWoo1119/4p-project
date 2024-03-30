using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StateDetailHover : MonoBehaviour
{
    public GameObject actObj;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    
    //y 좌표 추가 값

    private float a;

    //모든 hover 오브젝트
    private GameObject[] tgtObj; 
    private bool isHovering = false;

    // Start is called before the first frame update
    void Start()
    {
        //기본 스케일 설정
        originalScale = new Vector3(1, 1, 1);

        actObj.transform.localScale = new Vector3(0, 0, 0);

        tgtObj = GameObject.FindGameObjectsWithTag("StatHoverBox");
        int j = tgtObj.Length;
        for(int i = 0; i < j; i++)
        {
            EventTrigger trigger = tgtObj[i].gameObject.AddComponent<EventTrigger>();

            // PointerEnter �̺�Ʈ �߰�
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnPointerEnter(); });
            trigger.triggers.Add(entryEnter);

            // PointerExit �̺�Ʈ �߰�
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnPointerExit(); });
            trigger.triggers.Add(entryExit);
        }
    }

    void Update() 
    {
        //호버 위치 각 박스마다 고정(03.20)'
        float yRect = Input.mousePosition.y;
        if(yRect > 550)
        {
            a = 800;
        }
        else if(yRect <= 550)
        {
            a = 280;
        }
        actObj.GetComponent<RectTransform>().localPosition = new Vector3(Input.mousePosition.x - 900, Input.mousePosition.y - a, Input.mousePosition.z);
    }

    public void OnPointerEnter()
    {
        isHovering = true;
        actObj.transform.localScale = originalScale;
        //actObj.GetComponent<RectTransform>().localPosition = new Vector3(Input.mousePosition.x - 900, Input.mousePosition.y - 850, Input.mousePosition.z);
    }

    // ���콺�� ������ ��
    public void OnPointerExit()
    {
        isHovering = false;
        actObj.transform.localScale = new Vector3(0, 0, 0);
    }
}
