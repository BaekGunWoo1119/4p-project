using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StateDetailHover : MonoBehaviour
{
    public GameObject actObj;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    //오브젝트 이름 인식 관련 코드
    private string objName;

    //텍스트 코드
    public TMP_Text itemName;
    
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

        objName = actObj.ToString();
        int j = tgtObj.Length;
        for(int i = 0; i < j; i++)
        {
            EventTrigger trigger = tgtObj[i].gameObject.AddComponent<EventTrigger>();
            int index = i;

            // PointerEnter �̺�Ʈ �߰�
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnPointerEnter(index); });
            trigger.triggers.Add(entryEnter);

            // PointerExit �̺�Ʈ �߰�
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnPointerExit(index); });
            trigger.triggers.Add(entryExit);
        }
    }

    void Update() 
    {
        //호버 위치 각 박스마다 고정(03.20)'
        float yRect = Input.mousePosition.y;
        if(yRect > 600)
        {
            a = 830;
        }
        else if(yRect <= 600)
        {
            a = 220;
        }

        if (isHovering)
        {
            actObj.GetComponent<RectTransform>().localPosition = new Vector3(Input.mousePosition.x - 900, Input.mousePosition.y - a, Input.mousePosition.z);
        }
        
        if(itemName.text == "이름")
        {
            actObj.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void OnPointerEnter(int index)
    {
        isHovering = true;
        if(actObj.transform.localScale != originalScale)
        {
            actObj.transform.localScale = originalScale;
        }
        //호버링 시 설명 추가
        tgtObj[index].transform.parent.Find("HoverBox").GetComponent<TextChange>().ItemTextChange();
        Debug.Log(index + "번 박스 호버");
    }

    // ���콺�� ������ ��
    public void OnPointerExit(int index)
    {
        isHovering = false;
        actObj.transform.localScale = new Vector3(0, 0, 0);
        tgtObj[index].transform.parent.Find("HoverBox").GetComponent<TextChange>().ItemTextReset();
        Debug.Log(index + "번 박스 호버 취소");
    }
}
