using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class StateScroll : MonoBehaviour
{
    public Image[] scrollImg;
    public Sprite[] changeImg;
    public InventoryCtrl InvenCtrl;
    private bool isScroll = true;

    void Start()
    {
        for(int i = 0; i < scrollImg.Length; i++)
        {
            int index = i;
            EventTrigger trigger = scrollImg[i].gameObject.AddComponent<EventTrigger>();

            // PointerEnter 이벤트 추가
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data, index); });
            trigger.triggers.Add(entryEnter);

            // PointerExit 이벤트 추가
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data, index); });
            trigger.triggers.Add(entryExit);

            // Button 컴포넌트를 추가하거나 가져옵니다.
            Button button = scrollImg[i].gameObject.GetComponent<Button>();
            if (button == null)
            {
                button = scrollImg[i].gameObject.AddComponent<Button>();
            }

            // 클릭 이벤트 추가
            button.onClick.AddListener(Scroll);
        }
    }

    void Update()
    {
        if(InvenCtrl == null){
            InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
        }
        if(InvenCtrl.StatPoint == 0 && isScroll == true)
        {
            for(int i = 0; i < scrollImg.Length; i++)
            {
                scrollImg[i].gameObject.SetActive(false);
            }
            isScroll = false;

        }
        else if(isScroll == true)
        {
            for(int i = 0; i < scrollImg.Length; i++)
            {
                scrollImg[i].gameObject.SetActive(true);
            }
        }
        else if(InvenCtrl.StatPoint != 0)
        {
            isScroll = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData , int idx)
    {
        scrollImg[idx].sprite = changeImg[1];
    }

    public void OnPointerExit(PointerEventData eventData , int idx)
    {
        scrollImg[idx].sprite = changeImg[0];
    }

    public void Scroll()
    {
        InvenCtrl.StatPoint = InvenCtrl.StatPoint - 1; 
    }
}
