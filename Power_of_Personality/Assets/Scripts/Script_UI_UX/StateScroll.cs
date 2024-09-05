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
                // 클릭 이벤트 추가 위치 변경(09.04)
                button.onClick.AddListener(() => Scroll(index));
            }
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

    public void Scroll(int idx)
    {
        //스탯 스크롤 수정(09.04)
        Debug.Log(idx);
        InvenCtrl.StatPoint = InvenCtrl.StatPoint - 1; 
        if(idx == 0)
        {
            Status.TotalAD += 10f;
            Debug.Log(Status.TotalAD);
        }
        else if(idx == 1)
        {
            Status.TotalArmor += 10f;
            Debug.Log(Status.TotalArmor);
        }
        else if(idx == 2)
        {
            Status.TotalADC += 10f;
            Debug.Log(Status.TotalADC);
        }
        else if(idx == 3)
        {
            Status.TotalAP += 10f;
            Debug.Log(Status.TotalAP);
        }
        else if(idx == 4)
        {
            Status.TotalFire += 10f;
            Debug.Log(Status.TotalFire);
        }
        else if(idx == 5)
        {
            Status.TotalIce += 10f;
            Debug.Log(Status.TotalIce);
        }
        else if(idx == 6)
        {
            Status.TotalSpeed += 10f;
            Debug.Log(Status.TotalSpeed);
        }
        else if(idx == 7)
        {
            Status.TotalCooltime += 10f;
            Debug.Log(Status.TotalCooltime);
        }

    }
}
