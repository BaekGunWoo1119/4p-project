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
    //������Ʈ �̸� �ν� ���� �ڵ�
    private string objName;

    //�ؽ�Ʈ �ڵ�
    public TMP_Text itemName;
    
    //y ��ǥ �߰� ��

    private float a;

    //��� hover ������Ʈ
    private GameObject[] tgtObj; 
    private bool isHovering = false;

    private float screenWidth;
    private float screenHeight;

    // Start is called before the first frame update
    void Start()
    {
        //�⺻ ������ ����
        originalScale = new Vector3(1, 1, 1);

        actObj.transform.localScale = new Vector3(0, 0, 0);

        tgtObj = GameObject.FindGameObjectsWithTag("StatHoverBox");

        // 현재 화면의 크기 정보 가져오기
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        objName = actObj.ToString();
        int j = tgtObj.Length;
        for(int i = 0; i < j; i++)
        {
            EventTrigger trigger = tgtObj[i].gameObject.AddComponent<EventTrigger>();
            int index = i;

            // PointerEnter ???? ???
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnPointerEnter(index); });
            trigger.triggers.Add(entryEnter);

            // PointerExit ???? ???
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnPointerExit(index); });
            trigger.triggers.Add(entryExit);
        }
    }

    void Update() 
    {
        if(screenWidth != Screen.width)
            screenWidth = Screen.width;
        if(screenHeight != Screen.height)
            screenHeight = Screen.height;
            
        // 호버 시 따라다니게
        float yRect = Input.mousePosition.y;
        if (yRect > screenHeight * 0.537f)  // 580 / 1080을 비율로 계산
        {
            a = screenHeight * 0.6f;      // 820 / 1080을 비율로 계산
        }
        else if (yRect <= screenHeight * 0.537f)  // 580 / 1080
        {
            a = screenHeight * 0.3f;      // 210 / 1080
        }

        if (isHovering)
        {
            // 마우스 위치에 맞게 actObj의 위치를 설정 (화면 비율 고려)
            actObj.GetComponent<RectTransform>().localPosition = new Vector3(
                (Input.mousePosition.x / screenWidth) * 1920 - 900,  // x 좌표 비율 조정
                (Input.mousePosition.y / screenHeight) * 1080 - a,   // y 좌표 비율 조정
                Input.mousePosition.z
            );
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
        //ȣ���� �� ���� �߰�
        tgtObj[index].transform.parent.Find("HoverBox").GetComponent<TextChange>().ItemTextChange();
        //Debug.Log(index + "�� �ڽ� ȣ��");
    }

    // ???�J?? ?????? ??
    public void OnPointerExit(int index)
    {
        isHovering = false;
        actObj.transform.localScale = new Vector3(0, 0, 0);
        tgtObj[index].transform.parent.Find("HoverBox").GetComponent<TextChange>().ItemTextReset();
        Debug.Log(index + "번 박스 호버 취소");
    }
}
