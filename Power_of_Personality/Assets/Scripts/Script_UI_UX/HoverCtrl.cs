using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageHoverCtrl : MonoBehaviour
{
    public GameObject activeObj; // 활성화 시킬 오브젝트
    private Image imageComponent; // 대상 텍스트
    private Vector3 originalScale; // 오브젝트의 원래 스케일
    private Vector3 originalPosition; // 오브젝트의 원래 포지션
    private bool isHovering = false; // 마우스 호버 감지

    void Start()
    {
        originalPosition = activeObj.transform.position;
        originalScale = activeObj.transform.localScale;
        activeObj.transform.localScale = new Vector3(0, 0, 0);
        // Image 컴포넌트를 가져옴
        imageComponent = GetComponent<Image>();

        // EventTrigger 컴포넌트 추가
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        // PointerEnter 이벤트 추가
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnter(); });
        trigger.triggers.Add(entryEnter);

        // PointerExit 이벤트 추가
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointerExit(); });
        trigger.triggers.Add(entryExit);
    }

    // 마우스를 올렸을 때
    public void OnPointerEnter()
    {
        isHovering = true;
        activeObj.transform.localScale = originalScale; // 원래 스케일로 되돌림
    }

    // 마우스를 내렸을 때
    public void OnPointerExit()
    {
        isHovering = false;
        activeObj.transform.localScale = new Vector3(0, 0, 0); // 스케일 0 처리
    }
}
